using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poc.Data.Application.Services;
using Poc.RCL.Models;

namespace Poc.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IKpiService _kpiService;
        public List<KpiGroupDto> KpiGroups { get; set; } = new();
        public MessagesTotalDto? MessagesTotalDto { get; private set; }
        public List<MessageDto> Messages { get; private set; } = new();

        public IndexModel(IKpiService kpiService)
        {
            _kpiService = kpiService;
        }

        public async Task OnGetAsync()
        {
            var kpiTask      = _kpiService.GetDashboardGroupsAsync();
            var totalTask    = _kpiService.GetMessagesTotalAsync();
            var messagesTask = _kpiService.GetMessagesAsync();

            await Task.WhenAll(kpiTask, totalTask, messagesTask);

            KpiGroups        = kpiTask.Result;
            MessagesTotalDto = totalTask.Result;
            Messages         = messagesTask.Result;
        }

        public async Task<JsonResult> OnGetFilteredMessagesAsync([FromQuery] string[]? types)
        {
            var all = await _kpiService.GetMessagesAsync();
            var filtered = types?.Length > 0
                ? all.Where(m => types.Contains(m.MessageTypeCode)).ToList()
                : all;
            return new JsonResult(filtered);
        }
    }
}
