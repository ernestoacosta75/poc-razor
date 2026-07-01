using Poc.RCL.Models;

namespace Poc.Data.Application.Services
{
    public interface IKpiService
    {
        Task<MessagesTotalDto> GetMessagesTotalAsync();
        Task<List<KpiGroupDto>> GetDashboardGroupsAsync();
        Task<List<MessageDto>> GetMessagesAsync(string[]? types = null);
        Task<MessageDetailDto?> GetMessageDetailAsync(int id);
        Task<AttachmentFileDto?> GetAttachmentFileAsync(int messageId, int attachmentIndex);
    }
}
