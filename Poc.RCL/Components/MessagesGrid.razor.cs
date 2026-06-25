using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class MessagesGrid : IAsyncDisposable
    {
        [Parameter]
        public List<MessageDto>? Data { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        private IGrid? GridInstance { get; set; }
        private HubConnection? _hubConnection;
        private IJSObjectReference? _jsModule;
        private DotNetObjectReference<MessagesGrid>? _dotNetRef;
        private List<MessageDto> _messages = [];

        private int TotalCount => Data?.Count ?? 0;
        private int VisibleCount => _messages.Count;

        protected bool SyncSortAsc { get; set; } = false;

        private record TypeConfig(string Color, string Bg, string Icon);

        private string _dataScriptId = $"grid-data-{Guid.NewGuid().ToString("N")[..8]}";

        protected override void OnParametersSet()
        {
            _messages = Data ?? [];
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            // Connessione all'hub SignalR per ricevere i dati filtrati
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/hubs/messagegrid"))
                .Build();

            _hubConnection.On<List<MessageDto>>("ReceiveFilteredMessages", async (filtered) =>
            {
                _messages = filtered;
                await InvokeAsync(StateHasChanged);
            });

            await _hubConnection.StartAsync();

            // Registrazione del listener DOM per l'evento kpi-card-filter
            _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Poc.RCL/messagesGrid.js");
            _dotNetRef = DotNetObjectReference.Create(this);
            await _jsModule.InvokeVoidAsync("addKpiCardFilter", _dotNetRef);
        }

        [JSInvokable]
        public async Task OnKpiCardFilter(string[] types)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
                await _hubConnection.InvokeAsync("FilterMessages", types);
        }

        protected void ToggleSyncDateSort()
        {
            if (GridInstance == null) return;

            SyncSortAsc = !SyncSortAsc;
            var order = SyncSortAsc ? GridColumnSortOrder.Ascending : GridColumnSortOrder.Descending;
            GridInstance.SortBy("SyncDate", order);
        }

        protected async Task ClearGridFilter()
        {
            GridInstance?.ClearFilter();

            if (_hubConnection?.State == HubConnectionState.Connected)
                await _hubConnection.InvokeAsync("FilterMessages", Array.Empty<string>());
        }

        public async ValueTask DisposeAsync()
        {
            if (_jsModule != null)
            {
                await _jsModule.InvokeVoidAsync("removeKpiCardFilter");
                await _jsModule.DisposeAsync();
            }
            _dotNetRef?.Dispose();

            if (_hubConnection != null)
                await _hubConnection.DisposeAsync();
        }

        private TypeConfig GetTypeConfig(string type)
        {
            return (type?.ToLower().Trim()) switch
            {
                "eosp" => new TypeConfig("#0891b2", "#e0f2fe", "bi bi-stop-circle-fill"),
                "cosp" => new TypeConfig("#0ea5e9", "#dbeafe", "bi bi-play-circle-fill"),
                "sailing" => new TypeConfig("#3b82f6", "#eff6ff", "bi bi-tsunami"),
                "berthing" => new TypeConfig("#10b981", "#ecfdf5", "bi bi-geo-alt-fill"),
                var t when t != null && t.Contains("transit") =>
                    new TypeConfig("#8b5cf6", "#f5f3ff", "bi bi-arrow-left-right"),
                "refueling" => new TypeConfig("#f59e0b", "#fffbeb", "bi bi-fuel-pump"),
                "sludge" => new TypeConfig("#78716c", "#f5f5f4", "bi bi-trash"),
                var t when t != null && t.Contains("egcs") =>
                    new TypeConfig("#06b6d4", "#ecfeff", "bi bi-wind"),
                "at sea" => new TypeConfig("#0ea5e9", "#f0f9ff", "bi bi-water"),
                "at port" => new TypeConfig("#64748b", "#f8fafc", "bi bi-building"),
                var t when t != null && t.Contains("delay") =>
                    new TypeConfig("#ef4444", "#fef2f2", "bi bi-hourglass-split"),
                "survey" => new TypeConfig("#84cc16", "#f7fee7", "bi bi-clipboard-check"),
                "shore power" => new TypeConfig("#8b5cf6", "#faf5ff", "bi bi-lightning-charge"),
                "bunker req." => new TypeConfig("#f97316", "#fff7ed", "bi bi-file-earmark-text"),
                _ => new TypeConfig("#6b7280", "#f3f4f6", "bi bi-circle")
            };
        }
    }
}
