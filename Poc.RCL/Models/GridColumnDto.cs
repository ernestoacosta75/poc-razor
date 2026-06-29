namespace Poc.RCL.Models
{
    public class GridColumnDto
    {
        public string DataField { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string? Width { get; set; }
        public string? Alignment { get; set; }
        public string? DataType { get; set; }
        public bool Visible { get; set; } = true;

        // Queste properties sono per il template delle celle
        public string? TemplateType { get; set; } // "AvatarWithText", "Badge", "SimpleText"
        public string? BadgeBackgroundColor { get; set; } // Per il tipo "Badge"
        public string? BadgeTextColor { get; set; }       // Per il tipo "Badge"
        public string? AvatarColorField { get; set; }     // Campo del record che contiene il colore (es: "vesselColor")
        public string? AvatarTextField { get; set; }      // Campo del record per le iniziali (es: "vesselInitials")
        public string? SubTitleField { get; set; }

        public string? IconField { get; set; }         // Per le icone del TYPE (es: "messageIcon")
        public string? ToPortField { get; set; }       // Per la colonna ROUTE (es: "toPort")
        public string? RouteTypeField { get; set; }    // Se "sea" (bandiera gialla) o "land" (triangolo)
        public string? TimeField { get; set; }         // Ora del messaggio (es: "messageTime")
        public string? SyncDateField { get; set; }     // Data di sincronizzazione (es: "syncDateFormatted")
    }
}
