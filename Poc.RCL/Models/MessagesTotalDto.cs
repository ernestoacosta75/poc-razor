namespace Poc.RCL.Models
{
    public record MessagesTotalDto(int TodayCount, int YesterdayCountAtSameHour, List<int> HourlyTrendPoints);
}
