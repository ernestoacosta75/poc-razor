using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Poc.RCL.Models;

namespace Poc.Web.Pages
{
    public partial class MessagesGridWrapper
    {
        [Parameter] public List<MessageDto> InitialMessages { get; set; } = new();

        private static MessagesGridWrapper? CurrentInstance;
        private List<MessageDto> FilteredMessages = new();
        private bool IsDescending = true;
        private IGrid? GridInstance;

        protected override void OnInitialized()
        {
            // Sovrascrive sempre per agganciarsi all'istanza interattiva finale
            CurrentInstance = this;
            FilteredMessages = new List<MessageDto>(InitialMessages);
            EseguiOrdinamentoInMemoria();
        }

        [JSInvokable("FiltraggioDaTagHelper")]
        public static async Task FiltraggioDaTagHelper(string[] codiciFiltro)
        {
            if (CurrentInstance != null)
            {
                if (codiciFiltro == null || codiciFiltro.Length == 0)
                {
                    CurrentInstance.FilteredMessages = new List<MessageDto>(CurrentInstance.InitialMessages);
                }
                else
                {
                    CurrentInstance.FilteredMessages = CurrentInstance.InitialMessages
                        .Where(m => codiciFiltro.Contains(m.MessageType))
                        .ToList();
                }

                // Applica l'ordinamento
                CurrentInstance.IsDescending = CurrentInstance.IsDescending;
                if (CurrentInstance.IsDescending)
                    CurrentInstance.FilteredMessages = CurrentInstance.FilteredMessages.OrderByDescending(m => m.SyncDate).ToList();
                else
                    CurrentInstance.FilteredMessages = CurrentInstance.FilteredMessages.OrderBy(m => m.SyncDate).ToList();

                // FIX CRENTE: Forza il thread di Blazor Server a fare il re-render dell'UI
                await CurrentInstance.InvokeAsync(CurrentInstance.StateHasChanged);
            }
        }

        [JSInvokable("InvertiOrdinamentoData")]
        public static async Task InvertiOrdinamentoData()
        {
            if (CurrentInstance != null)
            {
                CurrentInstance.IsDescending = !CurrentInstance.IsDescending;

                if (CurrentInstance.IsDescending)
                    CurrentInstance.FilteredMessages = CurrentInstance.FilteredMessages.OrderByDescending(m => m.SyncDate).ToList();
                else
                    CurrentInstance.FilteredMessages = CurrentInstance.FilteredMessages.OrderBy(m => m.SyncDate).ToList();

                // FIX CRENTE: Forza il thread di Blazor Server a fare il re-render dell'UI
                await CurrentInstance.InvokeAsync(CurrentInstance.StateHasChanged);
            }
        }

        private void EseguiOrdinamentoInMemoria()
        {
            if (IsDescending)
                FilteredMessages = FilteredMessages.OrderByDescending(m => m.SyncDate).ToList();
            else
                FilteredMessages = FilteredMessages.OrderBy(m => m.SyncDate).ToList();

            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Aggiorna contatore e freccia nella toolbar superiore del CSHTML
            await JS.InvokeVoidAsync("AggiornaContatoreToolbar", FilteredMessages.Count);
            await JS.InvokeVoidAsync("AggiornaFrecciaOrdinamento", IsDescending);
        }
    }
}
