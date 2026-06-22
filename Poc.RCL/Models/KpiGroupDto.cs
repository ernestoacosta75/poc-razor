namespace Poc.RCL.Models
{
    public record KpiGroupDto(
        string Title, 
        string HeaderIcon, 
        string AccentColor, 
        List<MetricKpiDto> Metrics,
        bool IsGrid = false
    );
}
