using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class MessagesGrid
    {
        [Parameter]
        public List<MessageDto>? Data { get; set; }

        private string _dataScriptId = $"grid-data-{Guid.NewGuid().ToString("N")[..8]}";
    }
}
