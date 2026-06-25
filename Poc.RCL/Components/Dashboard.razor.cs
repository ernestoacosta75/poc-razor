using Microsoft.AspNetCore.Components;
using Poc.RCL.Models;

namespace Poc.RCL.Components
{
    public partial class Dashboard
    {
        [Parameter] public List<MessageDto> AllMessages { get; set; } = [];
        [Parameter] public List<KpiGroupDto> KpiGroups { get; set; } = [];
        [Parameter] public MessagesTotalDto? MessagesTotalDto { get; set; }

        private List<MessageDto> _allMessages = [];
        private List<MessageDto> _filteredMessages = [];

        protected override void OnParametersSet()
        {
            _allMessages = AllMessages;
            _filteredMessages = _allMessages;
        }

        private void HandleFilter(string[] types)
        {
            if (types == null || types.Length == 0)
            {
                _filteredMessages = _allMessages;
                return;
            }

            _filteredMessages = _allMessages
                .Where(m => types.Contains(m.MessageTypeCode))
                .ToList();
        }

        private void HandleClearFilter()
        {
            _filteredMessages = _allMessages;
        }
    }
}
