using Poc.Data.Application.Services;
using Poc.RCL.Models;

namespace Poc.Data.Infrastructure.Services
{
    public class KpiService : IKpiService
    {

        private static readonly List<MessageDto> _mockMessages = new();

        static KpiService()
        {
            var d = new DateTime(2026, 3, 20);
            _mockMessages = new List<MessageDto>
            {
                new(1,  "MSC GÜLSÜN",    "MG", "#0d9488", "gulsun.captain@msc.com",    "Sorrento", "FE402W", "EOSP",       d.AddHours(10).AddMinutes(30), "10:30", d.AddHours(12).AddMinutes(15), "ALGECIRAS",   "SHANGHAI",   false),
                new(2,  "MSC AMBRA",     "MA", "#3b82f6", "ambra.captain@msc.com",     "Limassol", "FE400E", "Sailing",    d.AddHours(10).AddMinutes(30), "10:30", d.AddHours(12).AddMinutes(0),  "BUSAN",       "ANTWERP",    false),
                new(3,  "MSC ELOANE",    "ME", "#10b981", "eloane.captain@msc.com",    "Limassol", "FE400W", "Berthing",   d.AddHours(9).AddMinutes(30),  "09:30", d.AddHours(11).AddMinutes(0),  "ANTWERP",     "GIOIA TAURO",false),
                new(4,  "MSC ELOANE",    "ME", "#10b981", "eloane.captain@msc.com",    "Limassol", "FE400E", "COSP",        d.AddHours(8).AddMinutes(30),  "08:30", d.AddHours(10).AddMinutes(0),  "LE HAVRE",    "SINGAPORE",  false),
                new(5,  "MSC ELOANE",    "ME", "#6366f1", "eloane.captain@msc.com",    "Limassol", "FE402E", "Transit",     d.AddHours(10).AddMinutes(33), "10:33", d.AddHours(10).AddMinutes(0),  "CORINTH CAN.","HONG KONG",  true),
                new(6,  "MSC BELLISSIMA","MB", "#8b5cf6", "bellissima.captain@msc.com","Genoa",    "FE301N", "Sailing",    d.AddHours(7).AddMinutes(45),  "07:45", d.AddHours(9).AddMinutes(30),  "GENOA",       "BARCELONA",  false),
                new(7,  "MSC DIVINA",    "MD", "#f59e0b", "divina.captain@msc.com",    "Marseille","FE201S", "EOSP",        d.AddHours(6).AddMinutes(0),   "06:00", d.AddHours(8).AddMinutes(45),  "DUBAI",       "ROTTERDAM",  false),
                new(8,  "MSC GRANDIOSA", "MG", "#f43f5e", "grandiosa.captain@msc.com", "Palermo",  "FE502E", "Transit",     d.AddHours(11).AddMinutes(15), "11:15", d.AddHours(13).AddMinutes(0),  "SUEZ",        "SHANGHAI",   true),
                new(9,  "MSC ORCHESTRA", "MO", "#0ea5e9", "orchestra.captain@msc.com", "Singapore","FE301W", "Refueling",   d.AddHours(14).AddMinutes(0),  "14:00", d.AddHours(15).AddMinutes(30), "SINGAPORE",   "PIRAEUS",    false),
                new(10, "MSC MERAVIGLIA","MM", "#06b6d4", "meraviglia.captain@msc.com","Valencia", "FE401E", "Sludge",      d.AddHours(9).AddMinutes(0),   "09:00", d.AddHours(10).AddMinutes(45), "VALENCIA",    "LE HAVRE",   false),
                new(11, "MSC MAGNIFICA", "MN", "#84cc16", "magnifica.captain@msc.com", "Rotterdam","FE201N", "EGCS",        d.AddHours(8).AddMinutes(0),   "08:00", d.AddHours(9).AddMinutes(15),  "ROTTERDAM",   "ALGECIRAS",  false),
            };
        }
        public async Task<MessagesTotalDto> GetMessagesTotalAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            int currentCount = _mockMessages.Count;

            // Ipotizziamo che ieri fossero ad esempio 8 per mantenere coerente
            // il trend positivo
            int yesterdayCount = 8;

            // Esempio di simulazione: Oggi alle 14:00 ci sono 22 msgs, ieri c'erano 18 ==> ((22 - 18) / 18) * 100 = +22.2%
            return new MessagesTotalDto(
                TodayCount: currentCount,
                YesterdayCountAtSameHour: yesterdayCount,
                HourlyTrendPoints: new List<int> { 5, 8, 12, 14, 18, 15, 22 } 
            );
        }

        public async Task<List<KpiGroupDto>> GetDashboardGroupsAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            return new List<KpiGroupDto>()
            {
                new KpiGroupDto(
                    Title: "Operations",
                    HeaderIcon: "bi bi-ship",
                    AccentColor: "#1e40af", // Blue
                    Metrics: new()
                    {
                        new MetricKpi("Sailing", ["SAI"], "2", "bi bi-moisture text-primary"),
                        new MetricKpi("Berthing", ["ARR"], "1", "bi bi-anchor text-primary"),
                    }
                ),
                new KpiGroupDto(
                    Title: "Noon Reports",
                    HeaderIcon: "bi bi-brightness-high",
                    AccentColor: "#8b5cf6", // Porpora
                    Metrics: new()
                    {
                        new MetricKpi("At Sea", ["IN1"], "0", "bi bi-waves"),
                        new MetricKpi("At Port", ["PNO"], "0", "bi bi-building"),
                    }
                ),
                new KpiGroupDto(
                    Title: "Sea Passages",
                    HeaderIcon: "bi bi-bezier2",
                    AccentColor: "#06b6d4", // Cyan
                    Metrics: new()
                    {
                        new MetricKpi("COSP", ["COSP"], "1", "bi bi-play-circle-fill text-info"),
                        new MetricKpi("EOSP", ["EOSP"], "2", "bi bi-stop-circle-fill text-info"),
                        new MetricKpi("Transit", ["EOT"], "2", "bi bi-arrow-left-right")
                    }
                ),
                new KpiGroupDto(
                    Title: "Fuel",
                    HeaderIcon: "bi bi-fuel-pump",
                    AccentColor: "#b45309", // Ambra
                    Metrics: new()
                    {
                        new MetricKpi("Refueling", ["BRF"], "1", "bi bi-droplet-fill text-warning"),
                        new MetricKpi("Bunker Req.", ["BUN"], "0", "bi bi-file-earmark-text")
                    }
                ),
                new KpiGroupDto(
                    Title: "Other",
                    HeaderIcon: "bi bi-check-circle",
                    AccentColor: "#10b981", // Verde
                    Metrics: new()
                    {
                        new MetricKpi("Delays", [], "0", "bi bi-hourglass-split"),
                        new MetricKpi("Sludge", ["SLP"], "1", "bi bi-trash"),
                        new MetricKpi("EGCS", ["EGCS"], "1", "bi bi-wind"),
                        new MetricKpi("Survey", [], "0", "bi bi-clipboard-check"),
                        new MetricKpi("Shore Power", ["SPW"], "0", "bi bi-lightning-charge")
                    },
                    IsGrid: true
                )
            };
        }

        public Task<List<MessageDto>> GetMessagesAsync()
        {
            return Task.FromResult(_mockMessages);
        }
    }
}
