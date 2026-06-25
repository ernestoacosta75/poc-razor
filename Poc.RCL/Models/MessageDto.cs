using System.ComponentModel.DataAnnotations;

namespace Poc.RCL.Models;

public record MessageDto
{
    // Costruttore esplicito per non rompere il mock esistente
    public MessageDto(int id, string vesselName, string vesselInitials, string vesselColor, string captainEmail, string office, string voyageCode, string messageTypeDesc, string messageTypeCode, DateTime messageDate, string messageTime, DateTime syncDate, string fromPort, string toPort, bool isTransitRoute)
    {
        Id = id;
        VesselName = vesselName;
        VesselInitials = vesselInitials;
        VesselColor = vesselColor;
        CaptainEmail = captainEmail;
        Office = office;
        VoyageCode = voyageCode;
        MessageTypeDesc = messageTypeDesc;
        MessageTypeCode = messageTypeCode;
        MessageDate = messageDate;
        MessageTime = messageTime;
        SyncDate = syncDate;
        FromPort = fromPort;
        ToPort = toPort;
        IsTransitRoute = isTransitRoute;
    }

    // Costruttore vuoto necessario se usi framework di serializzazione
    public MessageDto() { }

    public int Id { get; init; }
    public string VesselName { get; init; } = "";
    public string VesselInitials { get; init; } = "";
    public string VesselColor { get; init; } = "";
    public string CaptainEmail { get; init; } = "";
    public string Office { get; init; } = "";
    public string VoyageCode { get; init; } = "";
    public string MessageTypeDesc { get; init; } = "";
    public string MessageTypeCode { get; init; } = "";
    public DateTime MessageDate { get; init; }
    public string MessageTime { get; init; } = "";
    public DateTime SyncDate { get; init; }
    public string FromPort { get; init; } = "";
    public string ToPort { get; init; } = "";
    public bool IsTransitRoute { get; init; }
}
