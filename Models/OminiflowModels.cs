namespace FelterAPI.Models;

public class OmniFlowClient
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ClientType { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowFacility
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowSector
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string? Name { get; set; }
    public string? AccessLevel { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowPerson
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string? Name { get; set; }
    public string? Document { get; set; }
    public string? Type { get; set; }
    public string? PhotoBase64 { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowFaceTemplate
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public byte[]? Embedding { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowAccessEvent
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public Guid SectorId { get; set; }
    public string? EventType { get; set; }
    public string? DeviceName { get; set; }
    public DateTime OccurredAt { get; set; }
}
