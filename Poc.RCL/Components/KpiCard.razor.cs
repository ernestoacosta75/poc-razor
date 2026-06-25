using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class KpiCard
    {
        [Parameter]
        public KpiGroupDto Data { get; set; } = null!;

        [Parameter] public EventCallback<string[]> OnFilter { get; set; }

        private Task FilterAll() =>
            OnFilter.InvokeAsync(Data.Metrics.SelectMany(m => m.Code).ToArray());
    }
}
