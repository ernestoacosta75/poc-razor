using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class MetricKpiItem
    {
        [Parameter]
        [EditorRequired]
        public MetricKpi Item { get; set; } = default!;

        // Callback per notificare KpiCard
        [Parameter]
        public EventCallback<string[]> OnMetricClick { get; set; }

        private async Task GestisciClickMetrica()
        {
            if (Item.Code != null)
            {
                // Facciamo risalire l'array di stringhe
                await OnMetricClick.InvokeAsync(Item.Code);
            }
        }
    }
}
