using Geosys.NotificationDataExporter.WebHook.Domain.Models;

namespace Geosys.NotificationDataExporter.WebHook.Domain.Services
{
    public interface IAnalyticsService
    {
        Task OnNewAnalyticsAsync(List<AnalyticsDto> analytics, string? userId);
    }
}
