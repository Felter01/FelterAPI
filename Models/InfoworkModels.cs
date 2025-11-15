
namespace FelterAPI.Models;

public class InfoWorkClient
{
    public Guid Id { get; set; }

    // Dados básicos
    public string? Name { get; set; }
    
    public string? Cnpj { get; set; }

    // Login
    public string? Email { get; set; }
    
    public string? Password { get; set; }
    
    public bool IsActive { get; set; } = true;

    public string? Phone { get; set; }
    
    public string? Address { get; set; }
    
    public DateTime CreatedAt { get; set; }
}


public class InfoWorkComputer
{
    public Guid Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public string? Name { get; set; }
    
    public string? Status { get; set; }
    
    public string? Brand { get; set; }
    
    public string? Model { get; set; }
    
    public string? Serial { get; set; }
    
    public string? Os { get; set; }
    
    public string? Cpu { get; set; }
    
    public string? Ram { get; set; }
    
    public string? Storage { get; set; }
    
    public string? Notes { get; set; }
}

public class InfoWorkPassword
{
    public Guid Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public string? Service { get; set; }
    
    public string? UserName { get; set; }
    
    public string? Password { get; set; }
    
    public string? Url { get; set; }
    
    public string? Notes { get; set; }
}

public class InfoWorkRouter
{
    public Guid Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public string? Brand { get; set; }
    
    public string? Model { get; set; }
    
    public string? Serial { get; set; }
    
    public string? Ip { get; set; }
    
    public string? Location { get; set; }
    
    public string? AdminUser { get; set; }
    
    public string? AdminPass { get; set; }
    
    public string? WifiName { get; set; }
    
    public string? WifiPass { get; set; }
    
    public string? Status { get; set; }
}

public class InfoWorkEquipment
{
    public Guid Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public string? Name { get; set; }
    
    public string? Type { get; set; }
    
    public string? Brand { get; set; }
    
    public string? Model { get; set; }
    
    public string? Serial { get; set; }
    
    public string? Location { get; set; }
    
    public DateTime? PurchaseDate { get; set; }
    
    public DateTime? WarrantyEnd { get; set; }
    
    public string? Notes { get; set; }
    
    public string? Status { get; set; }
}

public class InfoWorkActivity
{
    public Guid Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? Category { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public class InfoWorkAlert
{
    public Guid Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? Priority { get; set; }
    
    public string? Status { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
