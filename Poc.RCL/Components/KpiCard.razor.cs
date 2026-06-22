using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class KpiCard
    {
        [Parameter]
        public KpiGroupDto Data { get; set; } = null!;
    }
}
