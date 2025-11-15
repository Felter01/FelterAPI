using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models;

public class StockFlowClient
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("master_id")]
    public Guid? MasterId { get; set; }

    // Dados básicos
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("cnpj")]
    public string? Cnpj { get; set; }

    // Login
    [Column("email")]
    public string? Email { get; set; }
    
    [Column("password")]
    public string? Password { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("phone")]
    public string? Phone { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}


public class StockFlowEmployee
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("email")]
    public string? Email { get; set; }
    
    [Column("password_hash")]
    public string? PasswordHash { get; set; }
    
    [Column("role")]
    public string? Role { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class StockFlowProduct
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Column("level_min")]
    public int LevelMin { get; set; }
    
    [Column("level_med")]
    public int LevelMed { get; set; }
    
    [Column("level_max")]
    public int LevelMax { get; set; }
    
    [Column("category")]
    public string? Category { get; set; }
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("photo_base64")]
    public string? PhotoBase64 { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class StockFlowRequest
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("product_id")]
    public Guid ProductId { get; set; }
    
    [Column("requester_id")]
    public Guid RequesterId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Column("status")]
    public string Status { get; set; } = "pendente";
    
    [Column("observation")]
    public string? Observation { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class StockFlowQuotation
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("request_id")]
    public Guid RequestId { get; set; }
    
    [Column("merchant_id")]
    public Guid MerchantId { get; set; }
    
    [Column("amount")]
    public decimal Amount { get; set; }
    
    [Column("note")]
    public string? Note { get; set; }
    
    [Column("status")]
    public string Status { get; set; } = "pendente";
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class StockFlowSetting
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("sms_enabled")]
    public bool SmsEnabled { get; set; }
    
    [Column("facial_enabled")]
    public bool FacialEnabled { get; set; }
    
    [Column("email_token_enabled")]
    public bool EmailTokenEnabled { get; set; } = true;
    
    [Column("language")]
    public string Language { get; set; } = "pt-BR";
}
