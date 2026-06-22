using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class TotalMessagesCard
    {
        [Parameter]
        [EditorRequired]
        public MessagesTotalDto? Data { get; set; }

        private string ElementId = $"dx-wave-{Guid.NewGuid().ToString("N")[..8]}";
    }
}
