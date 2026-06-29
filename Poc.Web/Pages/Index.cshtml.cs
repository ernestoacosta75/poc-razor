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
        public List<GridColumnDto> GridColumns { get; private set; } = new();
        public List<MessageDto> Messages { get; private set; } = new();

        public IndexModel(IKpiService kpiService)
        {
            _kpiService = kpiService;
        }

        public async Task OnGetAsync()
        {
            GridColumns = await _kpiService.GetGridConfigurationAsync();

            var kpiTask      = _kpiService.GetDashboardGroupsAsync();
            var totalTask    = _kpiService.GetMessagesTotalAsync();
            var messagesTask = _kpiService.GetMessagesAsync();

            await Task.WhenAll(kpiTask, totalTask, messagesTask);

            KpiGroups        = kpiTask.Result;
            MessagesTotalDto = totalTask.Result;
            Messages         = messagesTask.Result;
        }

        public async Task<IActionResult> OnGetFilteredMessagesAsync(string[] types)
        {
            // Recupera della lista filtrata dei messaggi
            var filteredMessages = await _kpiService.GetMessagesAsync(types);

            return new JsonResult(filteredMessages);
        }
    }
}
