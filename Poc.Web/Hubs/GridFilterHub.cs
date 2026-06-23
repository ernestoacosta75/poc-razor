using Microsoft.AspNetCore.SignalR;
using Poc.Data.Application.Services;

namespace Poc.Web.Hubs
{
    public class GridFilterHub : Hub
    {
        private readonly IKpiService _kpiService;

        public GridFilterHub(IKpiService kpiService)
        {
            _kpiService = kpiService;
        }

        /**
         * Questo metodo viene chiamato quando l'header di una KPI
         * o qualcuno dei suoi item viene cliccato. Filtra i messaggi sul server e invia il risultato al chiamante.
         */
        public async Task FilterMessages(IEnumerable<string> types)
        {
            var all = await _kpiService.GetMessagesAsync();

            var typeList = types.ToList();

            var filtered = typeList.Count == 0
                ? all
                : all.Where(m => typeList.Contains(m.MessageTypeCode)).ToList();

            await Clients.Caller.SendAsync("ReceiveFilteredMessages", filtered);
        }
    }
}
