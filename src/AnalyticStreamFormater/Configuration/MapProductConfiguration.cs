namespace AnalyticStreamFormater.Configuration
{
    public class MapProductConfiguration
    {
        private string _url;

        public string Url
        {
            get => _url;
            // / needed because of HttpClient issue
            set => _url = value.EndsWith("/") ? value : value + "/";
        }
    }
}
