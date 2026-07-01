using System.Globalization;
using System.Text;

namespace Poc.Data.Infrastructure.Services
{
    // Genera un PDF a singola pagina, valido ma minimale, per i download mock degli allegati .pdf.
    // Nessuna dipendenza esterna: scrive a mano gli oggetti PDF, il content stream e la tabella xref.
    internal static class MinimalPdfWriter
    {
        public static byte[] Build(IReadOnlyList<string> lines)
        {
            var contentBuilder = new StringBuilder();
            for (var i = 0; i < lines.Count; i++)
            {
                contentBuilder.Append(i == 0 ? "50 740 Td " : "0 -16 Td ");
                contentBuilder.Append('(').Append(EscapePdfText(lines[i])).Append(") Tj\n");
            }

            var streamContent = $"BT\n/F1 11 Tf\n{contentBuilder}ET";

            var objects = new[]
            {
                "<< /Type /Catalog /Pages 2 0 R >>",
                "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
                "<< /Type /Page /Parent 2 0 R /Resources << /Font << /F1 4 0 R >> >> /MediaBox [0 0 612 792] /Contents 5 0 R >>",
                "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
                $"<< /Length {streamContent.Length} >>\nstream\n{streamContent}\nendstream"
            };

            var pdf = new StringBuilder();
            pdf.Append("%PDF-1.4\n");

            var offsets = new List<int> { 0 };
            for (var i = 0; i < objects.Length; i++)
            {
                offsets.Add(pdf.Length);
                pdf.Append(i + 1).Append(" 0 obj\n").Append(objects[i]).Append("\nendobj\n");
            }

            var xrefOffset = pdf.Length;
            pdf.Append("xref\n0 ").Append(objects.Length + 1).Append("\n0000000000 65535 f \n");
            for (var i = 1; i <= objects.Length; i++)
            {
                pdf.Append(offsets[i].ToString("D10")).Append(" 00000 n \n");
            }

            pdf.Append("trailer\n<< /Size ").Append(objects.Length + 1).Append(" /Root 1 0 R >>\nstartxref\n")
               .Append(xrefOffset).Append("\n%%EOF");

            // Tutto il testo prodotto sopra e' garantito ASCII (vedi EscapePdfText), quindi 1 char = 1 byte:
            // coerente con il /Length dichiarato e con gli offset della tabella xref.
            return Encoding.ASCII.GetBytes(pdf.ToString());
        }

        private static string EscapePdfText(string text)
        {
            return RemoveDiacritics(text ?? string.Empty)
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }

                sb.Append(c < 128 ? c : '?');
            }

            return sb.ToString();
        }
    }
}
