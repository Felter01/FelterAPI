using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models;

public class OmniFlowClient
{
    [Column("id")]
    public Guid Id { get; set; }

    // Dados básicos
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("client_type")]
    public string? ClientType { get; set; }

    // Login
    [Column("email")]
    public string? Email { get; set; }
    
    [Column("password")]
    public string? Password { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}


public class OmniFlowFacility
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("address")]
    public string? Address { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowSector
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("facility_id")]
    public Guid FacilityId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("access_level")]
    public string? AccessLevel { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowPerson
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("document")]
    public string? Document { get; set; }
    
    [Column("type")]
    public string? Type { get; set; }
    
    [Column("photo_base64")]
    public string? PhotoBase64 { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowFaceTemplate
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("person_id")]
    public Guid PersonId { get; set; }
    
    [Column("embedding")]
    public byte[]? Embedding { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class OmniFlowAccessEvent
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("person_id")]
    public Guid PersonId { get; set; }
    
    [Column("sector_id")]
    public Guid SectorId { get; set; }
    
    [Column("event_type")]
    public string? EventType { get; set; }
    
    [Column("device_name")]
    public string? DeviceName { get; set; }
    
    [Column("occurred_at")]
    public DateTime OccurredAt { get; set; }
}
