namespace FelterAPI.Models;

public class StockFlowClient
{
    public Guid Id { get; set; }
    public Guid? MasterId { get; set; }

    // Dados básicos
    public string? Name { get; set; }
    public string? Cnpj { get; set; }

    // Login
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; } = true;

    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}


public class StockFlowEmployee
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class StockFlowProduct
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
    public int LevelMin { get; set; }
    public int LevelMed { get; set; }
    public int LevelMax { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? PhotoBase64 { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class StockFlowRequest
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid ProductId { get; set; }
    public Guid RequesterId { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; } = "pendente";
    public string? Observation { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class StockFlowQuotation
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid MerchantId { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = "pendente";
    public DateTime CreatedAt { get; set; }
}

public class StockFlowSetting
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public bool SmsEnabled { get; set; }
    public bool FacialEnabled { get; set; }
    public bool EmailTokenEnabled { get; set; } = true;
    public string Language { get; set; } = "pt-BR";
}
