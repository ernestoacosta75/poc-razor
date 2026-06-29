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

        // This property gets the filters from the URL
        [BindProperty(SupportsGet = true)]
        public string[]? Types { get; set; }

        public IndexModel(IKpiService kpiService)
        {
            _kpiService = kpiService;
        }

        public async Task OnGetAsync()
        {
            GridColumns = await _kpiService.GetGridConfigurationAsync();

            var kpiTask      = _kpiService.GetDashboardGroupsAsync();
            var totalTask    = _kpiService.GetMessagesTotalAsync();
            var messagesTask = _kpiService.GetMessagesAsync(Types);

            await Task.WhenAll(kpiTask, totalTask, messagesTask);

            KpiGroups        = kpiTask.Result;
            MessagesTotalDto = totalTask.Result;

            var allMessages = messagesTask.Result;
            Messages = messagesTask.Result;
        }

        public async Task<IActionResult> OnGetFilteredMessagesAsync(string[] types)
        {
            // Recupera solo la lista filtrata dei messaggi dal service
            var filteredMessages = await _kpiService.GetMessagesAsync(types);

            // Ritorna unicamente l'array JSON (niente HTML pesante rielaborato)
            return new JsonResult(filteredMessages);
        }

        public async Task<IActionResult> OnGetGridDataAsync(string[] types)
        {
            var messages = await _kpiService.GetMessagesAsync(types);

            // Restituisce solo il JSON dei messaggi, senza ricaricare la pagina HTML
            return new JsonResult(messages);
        }
    }
}
