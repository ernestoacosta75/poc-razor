using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class MetricKpiItem
    {
        [Parameter]
        [EditorRequired]
        public MetricKpi Item { get; set; } = default!;
    }
}
