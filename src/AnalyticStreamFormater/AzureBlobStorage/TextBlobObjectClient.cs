using System.Text;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace AnalyticStreamFormater.AzureBlobStorage
{
    internal class TextBlobObjectClient
    {
        private const int LEASE_TTL = 15;
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly BlobContainerClient _containerClient;
        private readonly BlockBlobClient _blockBlobClient;
        private readonly BlobLeaseClient _blobLeaseClient;

        public TextBlobObjectClient(string connectionString, string containerName, string blobObjectName)
        {
            _connectionString = connectionString;
            _containerName = containerName;

            _containerClient = new(_connectionString, _containerName);
            _containerClient.CreateIfNotExists();
            _blockBlobClient = _containerClient.GetBlockBlobClient(blobObjectName);
            _blobLeaseClient = _blockBlobClient.GetBlobLeaseClient();
        }

        public async Task<bool> BlobExistAsync()
        {
            return await _blockBlobClient.ExistsAsync();
        }
        
        public async Task AppendStringAsync(string content)
        {
            // One block per hour
            var currentBlockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.ToUniversalTime().ToString("yyyyMMddHH")));
            
            // Acquire lease to avoid concurency access
            BlobLease blobLease = await AcquireLease();
            BlobRequestConditions blobRequestConditions = new()
            {
                LeaseId = blobLease.LeaseId
            };

            // Determine the offset of the last written data of the current block
            BlockList blockList = null;
            List<string> blockIds = null;

            bool isNewBlock = false;
            try
            {
                blockList = _blockBlobClient.GetBlockListAsync().Result.Value;
            }
            catch { }

            BlobBlock blobBlock = default;
            long blobBlockStartOffset = 0;
            long blobBlockLenght = 0;
            if (blockList != null)
            {
                blobBlock = blockList.CommittedBlocks.FirstOrDefault(b => b.Name == currentBlockId);
                blockIds = blockList.CommittedBlocks.Select(b => b.Name).ToList();
            }

            // The block already exists
            if (blobBlock.Name != null)
            {
                foreach (var block in blockList.CommittedBlocks)
                {
                    if (block.Name != currentBlockId)
                    {
                        blobBlockStartOffset += block.SizeLong;
                    }
                    else
                    {
                        break;
                    }
                }

                blobBlockLenght = blobBlock.SizeLong;
            }

            // New block to create
            else
            {
                if (blockIds is null)
                {
                    blockIds = new() { currentBlockId };
                }
                else
                {
                    blockIds.Add(currentBlockId);
                }
                isNewBlock = true;
            }

            // Write the string to append at the end of the block
            using (MemoryStream ms = new())
            {
                if (!isNewBlock)
                {
                    // First get existing block content
                    var httpRange = new Azure.HttpRange(blobBlockStartOffset, blobBlockLenght);
                    var output = await _blockBlobClient.DownloadStreamingAsync(httpRange, blobRequestConditions);
                    await output.Value.Content.CopyToAsync(ms);
                }

                // Write content
                await ms.WriteAsync(new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(content)));
                ms.Seek(0, SeekOrigin.Begin);

                await _blockBlobClient.StageBlockAsync(currentBlockId, ms, conditions: blobRequestConditions);
            }

            await _blockBlobClient.CommitBlockListAsync(blockIds, new CommitBlockListOptions { Conditions = blobRequestConditions });
            await _blobLeaseClient.ReleaseAsync();
        }

        /// <summary>
        /// If the blob does not exist, it will be created
        /// </summary>
        /// <returns></returns>
        private async Task<BlobLease> AcquireLease()
        {
            BlobLease blobLease = null;
            var leaseAcquired = false;

            while (!leaseAcquired)
            {
                try
                {
                    blobLease = await _blobLeaseClient.AcquireAsync(TimeSpan.FromSeconds(LEASE_TTL));
                    if (blobLease == null)
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
                    else
                        leaseAcquired = true;
                }
                catch (RequestFailedException ex)
                {
                    if (ex.Status == 404)
                    {
                        // Create the blob/csv file
                        using var ms = new MemoryStream();
                        await _blockBlobClient.UploadAsync(ms, new BlobUploadOptions());
                    }
                    else if (ex.Status != 409)
                        throw;
                }
            }

            return blobLease;
        }
    }
}
