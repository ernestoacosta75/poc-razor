namespace Poc.RCL.Models
{
    public record KpiGroupDto(
        string Title, 
        string HeaderIcon, 
        string AccentColor, 
        List<MetricKpi> Metrics,
        bool IsGrid = false
    );
}
