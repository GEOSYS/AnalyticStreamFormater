using System.Net.Http.Headers;
using AnalyticStreamFormater.Configuration;
using AnalyticStreamFormater.Domain.Services;
using Microsoft.Extensions.Options;

namespace AnalyticStreamFormater.Clients
{
    public class MapProductClient : IMapProductClient
    {
        private readonly IOptions<MapProductConfiguration> _mapProductConfiguration;
        private readonly HttpClient _client;
        private readonly ILogger<MapProductClient> _logger;

        public MapProductClient(IOptions<MapProductConfiguration> mapProductConfiguration, HttpClient client, ILogger<MapProductClient> logger)
        {
            _mapProductConfiguration = mapProductConfiguration;

            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.BaseAddress = new Uri(_mapProductConfiguration.Value.Url);
            _logger = logger;
        }

        /// <summary>
        /// Returns null if MapProduct call is not successful
        /// </summary>
        /// <param name="token"></param>
        /// <param name="seasonFieldId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public Stream? GetNdviMap(string token, string seasonFieldId, string imageId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (var response = _client.GetAsync($"season-fields/{seasonFieldId}@ID/coverage/{imageId}/base-reference-map/INSEASON_NDVI/image.tiff.zip"))
            {
                if (response.Result.IsSuccessStatusCode)
                {
                    return response.Result.Content.ReadAsStream();
                }
                else
                {
                    _logger.LogWarning("Error calling MapProduct : " + response.Result.StatusCode + ". " + response.Result.RequestMessage?.RequestUri?.AbsoluteUri);
                    return null;
                }
            }
        }

    }
}
