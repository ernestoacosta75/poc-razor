using Poc.RCL.Models;

namespace Poc.Web.Pages
{
    public partial class Index
    {
        private List<KpiGroupDto> KpiGroups = new();
        private MessagesTotalDto? MessagesTotalDto;
        private List<MessageDto> Messages = new();
        private List<MessageDto> FilteredMessages = new();
        private bool IsDescending = true;

        protected override async Task OnInitializedAsync()
        {
            KpiGroups = await KpiService.GetDashboardGroupsAsync();
            MessagesTotalDto = await KpiService.GetMessagesTotalAsync();
            Messages = await KpiService.GetMessagesAsync();

            // CORREZIONE: Assegniamo i messaggi a FilteredMessages fin dall'inizio!
            FilteredMessages = new List<MessageDto>(Messages);

            EseguiOrdinamento();
        }

        public void ApplicaFiltro(string[] codiciDaFiltrare)
        {
            if (codiciDaFiltrare == null || codiciDaFiltrare.Length == 0)
            {
                FilteredMessages = new List<MessageDto>(Messages);
            }
            else
            {
                FilteredMessages = Messages
                    .Where(m => codiciDaFiltrare.Contains(m.MessageType))
                    .ToList();
            }

            EseguiOrdinamento();
            StateHasChanged();
        }

        private void ToggleSyncDateSort()
        {
            IsDescending = !IsDescending;
            EseguiOrdinamento();
            StateHasChanged();
        }

        private void EseguiOrdinamento()
        {
            if (IsDescending)
            {
                FilteredMessages = FilteredMessages.OrderByDescending(m => m.SyncDate).ToList();
            }
            else
            {
                FilteredMessages = FilteredMessages.OrderBy(m => m.SyncDate).ToList();
            }
        }

        private void ClearGridFilter()
        {
            FilteredMessages = new List<MessageDto>(Messages);
            EseguiOrdinamento();
            StateHasChanged();
        }
    }
}
