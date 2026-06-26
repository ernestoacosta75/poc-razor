using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class MetricKpiItem
    {
        [Parameter] public string IconColor { get; set; } = string.Empty;

        [Parameter]
        [EditorRequired]
        public MetricKpiDto Item { get; set; } = null!;
    }
}
