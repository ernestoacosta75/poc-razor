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
                new(1,  "MSC GÜLSÜN",    "MG", "#0d9488", "gulsun.captain@msc.com",    "Sorrento", "FE402W", "EOSP", "EOSP",       d.AddHours(10).AddMinutes(30), "10:30", d.AddHours(12).AddMinutes(15), "ALGECIRAS",   "SHANGHAI",   false),
                new(2,  "MSC AMBRA",     "MA", "#3b82f6", "ambra.captain@msc.com",     "Limassol", "FE400E", "Sailing", "SAI",    d.AddHours(10).AddMinutes(30), "10:30", d.AddHours(12).AddMinutes(0),  "BUSAN",       "ANTWERP",    false),
                new(3,  "MSC ELOANE",    "ME", "#10b981", "eloane.captain@msc.com",    "Limassol", "FE400W", "Berthing", "ARR",   d.AddHours(9).AddMinutes(30),  "09:30", d.AddHours(11).AddMinutes(0),  "ANTWERP",     "GIOIA TAURO",false),
                new(4,  "MSC ELOANE",    "ME", "#10b981", "eloane.captain@msc.com",    "Limassol", "FE400E", "COSP", "COSP",        d.AddHours(8).AddMinutes(30),  "08:30", d.AddHours(10).AddMinutes(0),  "LE HAVRE",    "SINGAPORE",  false),
                new(5,  "MSC ELOANE",    "ME", "#6366f1", "eloane.captain@msc.com",    "Limassol", "FE402E", "EOSP Transit", "EOT",     d.AddHours(10).AddMinutes(33), "10:33", d.AddHours(10).AddMinutes(0),  "CORINTH CAN.","HONG KONG",  true),
                new(6,  "MSC BELLISSIMA","MB", "#8b5cf6", "bellissima.captain@msc.com","Genoa",    "FE301N", "Sailing", "SAI",    d.AddHours(7).AddMinutes(45),  "07:45", d.AddHours(9).AddMinutes(30),  "GENOA",       "BARCELONA",  false),
                new(7,  "MSC DIVINA",    "MD", "#f59e0b", "divina.captain@msc.com",    "Marseille","FE201S", "EOSP", "EOSP",        d.AddHours(6).AddMinutes(0),   "06:00", d.AddHours(8).AddMinutes(45),  "DUBAI",       "ROTTERDAM",  false),
                new(8,  "MSC GRANDIOSA", "MG", "#f43f5e", "grandiosa.captain@msc.com", "Palermo",  "FE502E", "COSP Transit", "COT",     d.AddHours(11).AddMinutes(15), "11:15", d.AddHours(13).AddMinutes(0),  "SUEZ",        "SHANGHAI",   true),
                new(9,  "MSC ORCHESTRA", "MO", "#0ea5e9", "orchestra.captain@msc.com", "Singapore","FE301W", "Refueling", "BRF",   d.AddHours(14).AddMinutes(0),  "14:00", d.AddHours(15).AddMinutes(30), "SINGAPORE",   "PIRAEUS",    false),
                new(10, "MSC MERAVIGLIA","MM", "#06b6d4", "meraviglia.captain@msc.com","Valencia", "FE401E", "Sludge", "SDC",     d.AddHours(9).AddMinutes(0),   "09:00", d.AddHours(10).AddMinutes(45), "VALENCIA",    "LE HAVRE",   false),
                new(11, "MSC MAGNIFICA", "MN", "#84cc16", "magnifica.captain@msc.com", "Rotterdam","FE201N", "EGCS", "EWE",       d.AddHours(8).AddMinutes(0),   "08:00", d.AddHours(9).AddMinutes(15),  "ROTTERDAM",   null,  false),
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

            var messagesCount = _mockMessages
                .GroupBy(_ => _.MessageTypeCode)
                .ToDictionary(_ => _.Key, _ => _.Count());

            int GetCount(params string[] codes)
            {
                return codes.Sum(_ => messagesCount.GetValueOrDefault(_, 0));
            }

            return new List<KpiGroupDto>()
            {
                new KpiGroupDto(
                    Title: "Operations",
                    HeaderIcon: "bi bi-ship",
                    AccentColor: "#1e40af", // Blue
                    Metrics: new()
                    {
                        new MetricKpiDto("Sailing", ["SAI"], GetCount("SAI").ToString(), "bi bi-tsunami"),
                        new MetricKpiDto("Berthing", ["ARR"], GetCount("ARR").ToString(), "bi bi-geo-alt-fill"),
                    }
                ),
                new KpiGroupDto(
                    Title: "Noon Reports",
                    HeaderIcon: "bi bi-brightness-high-fill",
                    AccentColor: "#8b5cf6", // Porpora
                    Metrics: new()
                    {
                        new MetricKpiDto("At Sea", ["IN1"], GetCount("IN1").ToString(), "bi bi-water"),
                        new MetricKpiDto("At Port", ["PNO"], GetCount("PNO").ToString(), "bi bi-building"),
                    }
                ),
                new KpiGroupDto(
                    Title: "Sea Passages",
                    HeaderIcon: "bi bi-bezier2",
                    AccentColor: "#06b6d4", // Cyan
                    Metrics: new()
                    {
                        new MetricKpiDto("COSP", ["COSP"], GetCount("COSP").ToString(), "bi bi-play-circle-fill"),
                        new MetricKpiDto("EOSP", ["EOSP"], GetCount("EOSP").ToString(), "bi bi-stop-circle-fill"),
                        new MetricKpiDto("Transit", ["EOT", "COT", "NOT"], GetCount("EOT", "COT", "NOT").ToString(), "bi bi-arrow-left-right")
                    }
                ),
                new KpiGroupDto(
                    Title: "Fuel",
                    HeaderIcon: "bi bi-fuel-pump",
                    AccentColor: "#b45309", // Ambra
                    Metrics: new()
                    {
                        new MetricKpiDto("Refueling", ["BRF"], GetCount("BRF").ToString(), "bi bi-fuel-pump"),
                        new MetricKpiDto("Bunker Req.", ["BUN"], GetCount("BUN").ToString(), "bi bi-file-earmark-plus")
                    }
                ),
                new KpiGroupDto(
                    Title: "Other",
                    HeaderIcon: "bi bi-check-circle",
                    AccentColor: "#10b981", // Verde
                    Metrics: new()
                    {
                        new MetricKpiDto("Delays", ["DEL", "DSA", "DSP"], GetCount("DEL", "DSA", "DSP").ToString(), "bi bi-hourglass"),
                        new MetricKpiDto("Sludge", ["SDC", "SLP"], GetCount("SDC", "SLP").ToString(), "bi bi-trash-fill"),
                        new MetricKpiDto("EGCS", ["EWD", "EWE"], GetCount("EWD", "EWE").ToString(), "bi bi-wind"),
                        new MetricKpiDto("Survey", ["SUR"], GetCount("SUR").ToString(), "bi bi-clipboard-check-fill"),
                        new MetricKpiDto("Shore Power", ["SPW"], GetCount("SWP").ToString(), "bi bi-plug-fill")
                    },
                    IsGrid: true
                )
            };
        }

        public Task<List<MessageDto>> GetMessagesAsync(string[]? types = null)
        {
            if (types == null || types.Length == 0)
            {
                return Task.FromResult(_mockMessages);
            }

            var filteredMessages = _mockMessages
                .Where(m => types.Contains(m.MessageTypeCode))
                .ToList();

            return Task.FromResult(filteredMessages);
        }

        public async Task<List<GridColumnDto>> GetGridConfigurationAsync()
        {
            var columnsConfig = new List<GridColumnDto>  
            {
                // Template composto da: Avatar + Titolo + Sotto-titolo
                new()
                {
                    DataField = "vesselName", 
                    Caption = "VESSEL", 
                    Width = "25%", 
                    TemplateType = "AvatarWithText",
                    AvatarColorField = "vesselColor",
                    AvatarTextField = "vesselInitials",
                    SubTitleField = "captainEmail"
                },
                // Template a Badge, composto da: Teso in un rettangolo
                new()
                {
                    DataField = "voyageCode", 
                    Caption = "VOYAGE", 
                    Width = "15%", 
                    Alignment = "center", 
                    TemplateType = "Badge",
                    BadgeBackgroundColor = "#fef9c3",
                    BadgeTextColor = "#854d0e"
                },
                // Testo semplice (nessun template)
                new()
                {
                    DataField = "messageTypeDesc", 
                    Caption = "TYPE", 
                    Width = "160", 
                    Alignment = "center",
                    TemplateType = "TypePill",
                    IconField = "messageIcon",
                    BadgeBackgroundColor = "messageBgColor", 
                    BadgeTextColor = "messageTextColor"
                },
                new()
                {
                    DataField = "messageDate", 
                    Caption = "DATE", 
                    Width = "170",
                    TemplateType = "ComplexDate", 
                    TimeField = "messageTime", 
                    SyncDateField = "syncDateFormatted"
                },
                new()
                {
                    DataField = "fromPort", 
                    Caption = "ROUTE", 
                    Width = "35%",
                    TemplateType = "RouteFlow", ToPortField = "toPort", RouteTypeField = "routeType"
                },
                // Template a Badge
                new()
                {
                    DataField = "messageTypeDesc", 
                    Caption = "STATUS", 
                    Width = "15%", 
                    Alignment = "center", 
                    TemplateType = "Badge",
                    BadgeBackgroundColor = "#dcfce7",
                    BadgeTextColor = "#166534"
                },
            };

            return columnsConfig;
        }
    }
}
