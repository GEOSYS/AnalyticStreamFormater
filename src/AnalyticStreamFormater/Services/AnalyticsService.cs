using AnalyticStreamFormater.AzureBlobStorage;
using AnalyticStreamFormater.Domain.Models;
using AnalyticStreamFormater.Domain.Services;
using AnalyticStreamFormater.Serialization;

namespace AnalyticStreamFormater.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private const string DEFAULT_AZURE_CONTAINER_NAME = "myblobcontainername";

        private const string COVERAGE_SCHEMA_ID = "COVERAGE";

        private readonly AnalyticsCsvSerializer _csvSerializer;

        private readonly IIdentityServerClient _identityServer;

        private readonly IMapProductClient _mapProduct;

        private readonly IConfiguration _configuration;

        private readonly ILogger<AnalyticsService> _logger;

        public AnalyticsService(IIdentityServerClient identityServer, IMapProductClient mapProduct, IConfiguration configuration, ILogger<AnalyticsService> logger)
        {
            _csvSerializer = new(";");
            _identityServer = identityServer;
            _mapProduct = mapProduct;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analytics"></param>
        /// <param name="userId">Used to name the container on Azure blob Storage. If null, a default name is given</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task OnNewAnalyticsAsync(List<AnalyticsDto> analytics, string? userId)
        {
            if (analytics == null || analytics.Count == 0)
                throw new Exception("Input Analytics List is null or empty");

            userId ??= DEFAULT_AZURE_CONTAINER_NAME;

            // Set userId as lowercase to follow Azure container naming convention
            // TODO Strengthen verification to follow full Azure naming rules
            userId = userId.ToLower();

            foreach (var analytic in analytics)
            {
                await WriteAnalyticsToAzureBlobStorageAsync(analytic, userId);
            }
        }

        /// <summary>
        /// Write Analytics into Azure Blob Storage. Write analytics into a csv, and for coverage analytics we also write the ndvi geotiff
        /// </summary>
        /// <param name="analytic"></param>
        /// <param name="containerName">Azure blob container name</param>
        /// <returns></returns>
        public async Task WriteAnalyticsToAzureBlobStorageAsync(AnalyticsDto analytic, string containerName)
        {
            var azureBlobStorageConnStr = _configuration.GetValue<string>("Azure:BlobStorage:ConnectionString");

            var csvFileName = $"{analytic.Schema.Id}_{analytic.Schema.Version.Major}_{analytic.Schema.Version.Minor}.csv";

            // Get content to write
            var headers = _csvSerializer.GetHeaders(analytic);
            var row = _csvSerializer.GetRow(analytic);

            // Write analytics into a csv
            _logger.LogInformation("Writing csv to Azure Blob Storage : " + containerName + ", " + csvFileName);
            var textBlobObjectClient = new TextBlobObjectClient(azureBlobStorageConnStr, containerName, csvFileName);

            if (!textBlobObjectClient.BlobExistAsync().Result)
            {
                await textBlobObjectClient.AppendStringAsync(headers);
            }
            await textBlobObjectClient.AppendStringAsync(row);
            
            // Coverage case : we make a call to MapProduct to get Ndvi Map and store it in the Azure blob storage
            if (analytic.Schema.Id.ToUpper() == COVERAGE_SCHEMA_ID)
            {
                var token = _identityServer.GetToken();
                var ndviMap = _mapProduct.GetNdviMap(token, analytic.Entity.Id, analytic.Values["Id"].ToString());

                if (ndviMap != null)
                {
                    _logger.LogInformation("MapProduct successful call. Writing to Azure Blob Storage...");

                    // Construct a unique file name for the geotiff
                    var sb = DateTime.Now.ToString().GetMd5Hash();
                    var geotiffFileName = $"{analytic.Entity.Id}-{(DateTime)analytic.Values["Date"]:yyyy-MM-dd}-{sb}.zip";

                    // Write file
                    var geotiffBlobObjectClient = new GeotiffBlobObjectClient(azureBlobStorageConnStr, containerName, geotiffFileName);
                    await geotiffBlobObjectClient.WriteFileAsync(ndviMap);
                }
            }
        }
    }
}
