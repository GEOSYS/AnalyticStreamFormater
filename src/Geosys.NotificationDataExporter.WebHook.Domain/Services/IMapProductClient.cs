namespace Geosys.NotificationDataExporter.WebHook.Domain.Services
{
    public interface IMapProductClient
    {
        /// <summary>
        /// Returns null if MapProduct call is not successful
        /// </summary>
        /// <param name="token"></param>
        /// <param name="seasonFieldId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        Stream? GetNdviMap(string token, string seasonFieldId, string imageId);
    }
}
