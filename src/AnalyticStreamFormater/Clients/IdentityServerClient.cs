using AnalyticStreamFormater.Configuration;
using Geosys.NotificationDataExporter.WebHook.Domain.Services;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;

namespace AnalyticStreamFormater.Clients
{
    public class IdentityServerClient : IIdentityServerClient
    {
        private readonly IOptions<IdentityServerConfiguration> _identityServerConfiguration;
        private readonly HttpClient _client;
        private readonly FormUrlEncodedContent _content;

        public IdentityServerClient(IOptions<IdentityServerConfiguration> identityServerConfiguration, HttpClient client)
        {
            _identityServerConfiguration = identityServerConfiguration;

            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.BaseAddress = new Uri(_identityServerConfiguration.Value.Url);

            _content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["client_id"] = _identityServerConfiguration.Value.ClientId,
                ["client_secret"] = _identityServerConfiguration.Value.ClientSecret,
                ["username"] = _identityServerConfiguration.Value.UserLogin,
                ["password"] = _identityServerConfiguration.Value.UserPassword,
                ["scope"] = _identityServerConfiguration.Value.Scope,
                ["grant_type"] = _identityServerConfiguration.Value.GrantType
            });
        }

        public string GetToken()
        {
            var token = string.Empty;

            using (var response = _client.PostAsync(_identityServerConfiguration.Value.TokenEndPoint, _content))
            {
                if (response.Result.IsSuccessStatusCode)
                {
                    var jsonresult = JObject.Parse(response.Result.Content.AsString());
                    token = (string)jsonresult["access_token"];
                }
            }

            return token;
        }
    }
}
