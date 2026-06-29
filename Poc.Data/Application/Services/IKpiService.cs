using Poc.RCL.Models;

namespace Poc.Data.Application.Services
{
    public interface IKpiService
    {
        Task<MessagesTotalDto> GetMessagesTotalAsync();
        Task<List<KpiGroupDto>> GetDashboardGroupsAsync();
        Task<List<MessageDto>> GetMessagesAsync(string[]? types);
        Task<List<GridColumnDto>> GetGridConfigurationAsync();
    }
}
