namespace QuickStarted
{
    public interface IAnalyticsService
    {
        Task OnNewAnalyticsAsync(List<AnalyticsDto> analytics, string? userId);
    }
}
