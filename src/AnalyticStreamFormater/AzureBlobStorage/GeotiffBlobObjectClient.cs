using Azure.Storage.Blobs;

namespace AnalyticStreamFormater.AzureBlobStorage
{
    internal class GeotiffBlobObjectClient
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly BlobContainerClient _containerClient;
        private readonly string _blobObjectName;

        public GeotiffBlobObjectClient(string connectionString, string containerName, string blobObjectName)
        {
            _connectionString = connectionString;
            _containerName = containerName;

            _containerClient = new(_connectionString, _containerName);
            _containerClient.CreateIfNotExists();
            _blobObjectName = blobObjectName;
        }

        public async Task WriteFileAsync(Stream file)
        {
            await _containerClient.UploadBlobAsync(_blobObjectName, file);
        }

    }
}
