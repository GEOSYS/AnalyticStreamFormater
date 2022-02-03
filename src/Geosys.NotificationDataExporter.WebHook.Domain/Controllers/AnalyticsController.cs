using Geosys.NotificationDataExporter.WebHook.Domain.Models;
using Geosys.NotificationDataExporter.WebHook.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Geosys.NotificationDataExporter.WebHook.API.AzureDemo.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(ILogger<AnalyticsController> logger, IAnalyticsService analyticsService)
        {
            _logger = logger;
            _analyticsService = analyticsService;
        }

        [HttpPost]
        public async Task<IActionResult> NewAnalyticsAsync( [FromBody] List<AnalyticsDto> analytics, [FromHeader(Name = "x-user-id")] string? userId )
        {
            try
            {
                _logger.LogInformation($"Received {analytics.Count} analytics");
                await _analyticsService.OnNewAnalyticsAsync(analytics, userId);
                _logger.LogInformation($"Analytics ({analytics.Count}) processed");
            }
            catch( Exception ex )
            {
                _logger.LogError(ex, $"Unexpected error: {ex.Message}");
                return StatusCode(500, "Something went wrong..." + ex.Message);
            }

            return StatusCode(200);
        }
    }
}