using System.ComponentModel.DataAnnotations;

namespace Poc.RCL.Models;

public record MessageDto(
    int Id,
    [property: Display(Name = "VESSEL")]   string VesselName,
    [property: Display(Name = "INITIALS")] string VesselInitials,
    [property: Display(Name = "COLOR")]    string VesselColor,
    [property: Display(Name = "CAPTAIN")]  string CaptainEmail,
    [property: Display(Name = "OFFICE")]   string Office,
    [property: Display(Name = "VOYAGE")]   string VoyageCode,
    [property: Display(Name = "TYPE")]     string MessageType,
    [property: Display(Name = "DATE")]     DateTime MessageDate,
    [property: Display(Name = "TIME")]     string MessageTime,
    [property: Display(Name = "SYNC")]     DateTime SyncDate,
    [property: Display(Name = "ROUTE")]    string FromPort,
    [property: Display(Name = "TO")]       string ToPort,
    [property: Display(Name = "TRANSIT")]  bool IsTransitRoute
);
