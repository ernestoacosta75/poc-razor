using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class KpiCard
    {
        [Parameter]
        public KpiGroupDto Data { get; set; } = null!;

        // "Canale" di comunicazione verso la WebApp
        [Parameter]
        public EventCallback<string[]> OnFilterSelected { get; set; }

        [Inject] private IJSRuntime JS { get; set; } = null!;

        // Questo metodo serve per gestire il click sull'header della Card
        private async Task FiltraInteroGruppo()
        {
            var tuttiICodici = Data.Metrics
                .SelectMany(m => m.Code ?? Array.Empty<string>())
                .ToArray();

            // 1. Facciamo scattare l'EventCallback C# (se serve internamente a Blazor)
            await OnFilterSelected.InvokeAsync(tuttiICodici);

            // 2. SOLUZIONE: Generiamo l'evento Custom JS che Index.cshtml sta ascoltando!
            await DispatchaFiltroSuCustomEventJS(tuttiICodici);
        }

        // Questo metodo serve per gestire il click su un singolo item all'interno della Card
        private async Task FiltraSingolaMetrica(string[] codiciMetrica)
        {
            await OnFilterSelected.InvokeAsync(codiciMetrica);

            // Generiamo l'evento Custom JS anche per la singola metrica
            await DispatchaFiltroSuCustomEventJS(codiciMetrica);
        }

        private async Task DispatchaFiltroSuCustomEventJS(string[] codici)
        {
            // Questo piccolissimo script crea l'evento "kpi-card-filter" atteso dal listener nel CSHTML
            var jsCode = @"
                var ev = new CustomEvent('kpi-card-filter', { detail: { types: " + System.Text.Json.JsonSerializer.Serialize(codici) + @" } });
                document.dispatchEvent(ev);
            ";
            await JS.InvokeVoidAsync("eval", jsCode);
        }
    }
}
