namespace AnalyticStreamFormater.Configuration
{
    public class IdentityServerConfiguration
    {
        private string _url;

        public string Url
        {
            get => _url;
            // / needed because of HttpClient issue
            set => _url = value.EndsWith("/") ? value : value + "/";
        }

        public string TokenEndPoint { get; set; }

        public string UserLogin { get; set; }

        public string UserPassword { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; }

        public string GrantType { get; set; }
    }
}
