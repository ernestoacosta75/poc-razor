using Microsoft.AspNetCore.Components;
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

        // Questo metodo serve per gestire il click sull'header della Card
        private async Task FiltraInteroGruppo()
        {
            // CAMBIATO: Usa SelectMany per unire tutti i sotto-array string[] in un unico string[] piatto
            var tuttiICodici = Data.Metrics
                .SelectMany(m => m.Code ?? Array.Empty<string>())
                .ToArray();

            await OnFilterSelected.InvokeAsync(tuttiICodici);
        }

        // Questo metodo serve per gestire il click su un singolo item all'interno della Card
        private async Task FiltraSingolaMetrica(string[] codiciMetrica)
        {
            // Fa rimbalzare l'evento verso la WebApp
            await OnFilterSelected.InvokeAsync(codiciMetrica);
        }
    }
}
