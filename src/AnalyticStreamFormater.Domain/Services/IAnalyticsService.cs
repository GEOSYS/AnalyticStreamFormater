using AnalyticStreamFormater.Domain.Models;

namespace AnalyticStreamFormater.Domain.Services
{
    public interface IAnalyticsService
    {
        Task OnNewAnalyticsAsync(List<AnalyticsDto> analytics, string? userId);
    }
}
