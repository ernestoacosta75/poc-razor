using System.Text;
using Poc.Data.Application.Services;
using Poc.Data.Domain.Constants;
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

            // Ipotizziamo che ieri fossero ad esempio 8 per mantenere coerente il trend positivo
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

            var transitCodes = _mockMessages
                .Where(_ => _.IsTransitRoute)
                .Select(_ => _.MessageTypeCode)
                .Distinct()
                .ToArray();

            var egcsCodes = _mockMessages
                .Where(_ => _.MessageTypeDesc != null && _.MessageTypeDesc.Contains(Constants.EgcsGroup, StringComparison.OrdinalIgnoreCase))
                .Select(_ => _.MessageTypeCode)
                .Distinct()
                .ToArray();

            var delayCodes = _mockMessages
                .Where(_ => _.MessageTypeDesc != null && _.MessageTypeDesc.Contains(Constants.DelayGroup, StringComparison.OrdinalIgnoreCase))
                .Select(_ => _.MessageTypeCode)
                .Distinct()
                .ToArray();

            var sludgeCodes = _mockMessages
                .Where(_ => _.MessageTypeDesc != null && _.MessageTypeDesc.Contains(Constants.SludgeGroup, StringComparison.OrdinalIgnoreCase))
                .Select(_ => _.MessageTypeCode)
                .Distinct()
                .ToArray();

            int GetCount(params string[] codes)
            {
                return codes.Sum(_ => messagesCount.GetValueOrDefault(_, 0));
            }

            return new List<KpiGroupDto>()
            {
                new (
                    Title: "Operations",
                    HeaderIcon: "bi bi-backpack-fill",
                    AccentColor: "#1e40af",
                    Metrics: new()
                    {
                        new MetricKpiDto("Sailing", ["SAI"], GetCount("SAI").ToString(), "detailslayout"), 
                        new MetricKpiDto("Berthing", ["ARR"], GetCount("ARR").ToString(), "pinright"),
                    }
                ),
                new (
                    Title: "Noon Reports",
                    HeaderIcon: "sun", 
                    AccentColor: "#8b5cf6", 
                    Metrics: new()
                    {
                        new MetricKpiDto("At Sea", ["IN1"], GetCount("IN1").ToString(), "contentlayout"),
                        new MetricKpiDto("At Port", ["PNO"], GetCount("PNO").ToString(), "bi bi-buildings-fill"), 
                    }
                ),
                new (
                    Title: "Sea Passages",
                    HeaderIcon: "decreaselinespacing",
                    AccentColor: "#06b6d4", 
                    Metrics: new()
                    {
                        new MetricKpiDto("COSP", ["COSP"], GetCount("COSP").ToString(), "video"),
                        new MetricKpiDto("EOSP", ["EOSP"], GetCount("EOSP").ToString(), "indeterminatestate"),
                        new MetricKpiDto("Transit", transitCodes, GetCount(transitCodes).ToString(), "optionsoutline") 
                    }
                ),
                new (
                    Title: "Fuel",
                    HeaderIcon: "fill",
                    AccentColor: "#b45309",
                    Metrics: new()
                    {
                        new MetricKpiDto("Refueling", ["BRF"], GetCount("BRF").ToString(), "fill"),
                        new MetricKpiDto("Bunker Req.", ["BUN"], GetCount("BUN").ToString(), "textdocument")
                    }
                ),
                new (
                    Title: "Other",
                    HeaderIcon: "checkmarkcircle",
                    AccentColor: "#10b981", 
                    Metrics: new()
                    {
                        new MetricKpiDto("Delays", delayCodes, GetCount(delayCodes).ToString(), "bi bi-hourglass"),
                        new MetricKpiDto("Sludge", sludgeCodes, GetCount(sludgeCodes).ToString(), "trash"),
                        new MetricKpiDto("EGCS", egcsCodes, GetCount(egcsCodes).ToString(), "strike"),
                        new MetricKpiDto("Survey", ["SUR"], GetCount("SUR").ToString(), "cardcontent"),
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

        public Task<MessageDetailDto?> GetMessageDetailAsync(int id)
        {
            var message = _mockMessages.FirstOrDefault(m => m.Id == id);

            if (message is null)
            {
                return Task.FromResult<MessageDetailDto?>(null);
            }

            var rnd = new Random(id);

            var callsign = $"3F{(char)('A' + rnd.Next(0, 26))}{rnd.Next(1, 9)}";
            var imo = (9000000 + rnd.Next(0, 999999)).ToString();
            var berth = rnd.Next(1, 15);
            var eta = message.SyncDate.AddDays(rnd.Next(1, 4)).AddHours(rnd.Next(0, 12));
            var distance = rnd.Next(300, 5000);
            var draftFwd = Math.Round(10 + rnd.NextDouble() * 6, 1);
            var draftAft = Math.Round(draftFwd - rnd.Next(1, 3), 1);
            var cargo = rnd.Next(4000, 15000);
            var bunker = rnd.Next(300, 2000);
            var weatherOptions = new[] { "GOOD", "MODERATE", "ROUGH" };
            var weather = weatherOptions[rnd.Next(weatherOptions.Length)];

            var originalContent =
                $"""
                 VESSEL: {message.VesselName}
                 CALLSIGN: {callsign}
                 IMO: {imo}
                 VOYAGE: {message.VoyageCode}

                 SAILING REPORT
                 PORT: {message.FromPort}
                 BERTH: {berth}

                 SAILING DATE/TIME:
                 {message.MessageDate:dd/MM/yyyy} {message.MessageTime} LT

                 NEXT PORT: {message.ToPort ?? "N/A"}
                 ETA: {eta:dd/MM/yyyy HH:mm} LT
                 DISTANCE: {distance} NM

                 DRAFT FWD: {draftFwd:F1} M
                 DRAFT AFT: {draftAft:F1} M

                 CARGO: {cargo} TEU
                 BUNKER ROB: {bunker} MT

                 WEATHER: {weather}
                 SEA STATE: {weather}
                 """;

            var attachments = GenerateAttachments(message);

            var comments = new List<CommentDto>
            {
                new(Author: "Ops Team", Text: "Confirmed report received, no anomalies to signal.", CreatedDate: message.SyncDate.AddMinutes(-30)),
                new(Author: message.CaptainEmail, Text: "Report sent as per schedule.", CreatedDate: message.SyncDate.AddMinutes(-45))
            };

            string FormatDateTime(DateTime d) => d.ToString("dd MMM yyyy, HH:mm");

            var fieldChangePool = new List<FieldChangeDto>
            {
                new("ETA Date", FormatDateTime(eta.AddHours(-rnd.Next(2, 30))), FormatDateTime(eta)),
                new("Draft Fwd", $"{Math.Round(draftFwd - 0.3, 1):F1} M", $"{draftFwd:F1} M"),
                new("Draft Aft", $"{Math.Round(draftAft - 0.2, 1):F1} M", $"{draftAft:F1} M"),
                new("Cargo", $"{cargo - rnd.Next(50, 300)} TEU", $"{cargo} TEU"),
                new("Bunker ROB", $"{bunker + rnd.Next(20, 80)} MT", $"{bunker} MT"),
                new("Berth", Math.Max(1, berth - 1).ToString(), berth.ToString()),
                new("Distance", $"{distance + rnd.Next(50, 200)} NM", $"{distance} NM")
            };

            var syncHistory = new List<int> { 0, 15, 30, 45 }
                .Select(offset => new SyncHistoryEntryDto(
                    Date: message.SyncDate.AddMinutes(-offset),
                    Changes: fieldChangePool.OrderBy(_ => rnd.Next()).Take(rnd.Next(1, 3)).ToList()))
                .ToList();

            var detail = new MessageDetailDto(
                MessageId: message.Id,
                VesselName: message.VesselName,
                Voyage: message.VoyageCode,
                MessageType: message.MessageTypeDesc.ToUpperInvariant(),
                OriginalContent: originalContent,
                Attachments: attachments,
                Comments: comments,
                SyncHistory: syncHistory
            );

            return Task.FromResult<MessageDetailDto?>(detail);
        }

        public Task<AttachmentFileDto?> GetAttachmentFileAsync(int messageId, int attachmentIndex)
        {
            var message = _mockMessages.FirstOrDefault(m => m.Id == messageId);
            if (message is null)
            {
                return Task.FromResult<AttachmentFileDto?>(null);
            }

            var attachments = GenerateAttachments(message);
            if (attachmentIndex < 0 || attachmentIndex >= attachments.Count)
            {
                return Task.FromResult<AttachmentFileDto?>(null);
            }

            var attachment = attachments[attachmentIndex];
            var summaryLines = new[]
            {
                $"Attachment: {attachment.FileName}",
                $"Vessel: {message.VesselName} ({message.VoyageCode})",
                $"Size: {attachment.SizeLabel}",
                $"Uploaded: {attachment.UploadedDate:dd MMM yyyy, HH:mm}",
                "",
                "Placeholder file generated from mock data (no real attachment content is stored in this POC)."
            };

            var isPdf = attachment.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
            var content = isPdf
                ? MinimalPdfWriter.Build(summaryLines)
                : Encoding.UTF8.GetBytes(string.Join("\n", summaryLines));
            var contentType = isPdf ? "application/pdf" : "text/plain";

            return Task.FromResult<AttachmentFileDto?>(new AttachmentFileDto(content, contentType, attachment.FileName));
        }

        private static List<AttachmentDto> GenerateAttachments(MessageDto message)
        {
            var rnd = new Random(message.Id);
            var extensions = new[] { "pdf", "txt" };

            return Enumerable.Range(1, rnd.Next(0, 3))
                .Select(i => new AttachmentDto(
                    FileName: $"telex_{message.VoyageCode}_{i}.{extensions[(i - 1) % extensions.Length]}",
                    SizeLabel: $"{rnd.Next(80, 900)} KB",
                    UploadedDate: message.SyncDate.AddMinutes(-rnd.Next(5, 120))))
                .ToList();
        }
    }
}
