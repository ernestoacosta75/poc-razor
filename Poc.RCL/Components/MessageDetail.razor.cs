using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class MessageDetail
    {
        [Parameter]
        public MessageDetailDto Data { get; set; } = null!;

        private string _id = $"telex{Guid.NewGuid().ToString("N")[..8]}";
    }
}
