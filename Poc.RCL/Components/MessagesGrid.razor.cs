using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class MessagesGrid
    {
        [Parameter]
        public List<MessageDto>? Data { get; set; }

        [Parameter]
        public List<GridColumnDto>? Columns { get; set; }

        [Parameter]
        public string FilterUrl { get; set; } = string.Empty;

        private string _gridId = $"grid{Guid.NewGuid().ToString("N")[..8]}";
    }
}
