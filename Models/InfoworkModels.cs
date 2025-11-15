using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models;

public class InfoWorkClient
{
    [Column("id")]
    public Guid Id { get; set; }

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
    
    [Column("address")]
    public string? Address { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}


public class InfoWorkComputer
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("status")]
    public string? Status { get; set; }
    
    [Column("brand")]
    public string? Brand { get; set; }
    
    [Column("model")]
    public string? Model { get; set; }
    
    [Column("serial")]
    public string? Serial { get; set; }
    
    [Column("os")]
    public string? Os { get; set; }
    
    [Column("cpu")]
    public string? Cpu { get; set; }
    
    [Column("ram")]
    public string? Ram { get; set; }
    
    [Column("storage")]
    public string? Storage { get; set; }
    
    [Column("notes")]
    public string? Notes { get; set; }
}

public class InfoWorkPassword
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("service")]
    public string? Service { get; set; }
    
    [Column("user_name")]
    public string? UserName { get; set; }
    
    [Column("password")]
    public string? Password { get; set; }
    
    [Column("url")]
    public string? Url { get; set; }
    
    [Column("notes")]
    public string? Notes { get; set; }
}

public class InfoWorkRouter
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("brand")]
    public string? Brand { get; set; }
    
    [Column("model")]
    public string? Model { get; set; }
    
    [Column("serial")]
    public string? Serial { get; set; }
    
    [Column("ip")]
    public string? Ip { get; set; }
    
    [Column("location")]
    public string? Location { get; set; }
    
    [Column("admin_user")]
    public string? AdminUser { get; set; }
    
    [Column("admin_pass")]
    public string? AdminPass { get; set; }
    
    [Column("wifi_name")]
    public string? WifiName { get; set; }
    
    [Column("wifi_pass")]
    public string? WifiPass { get; set; }
    
    [Column("status")]
    public string? Status { get; set; }
}

public class InfoWorkEquipment
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("type")]
    public string? Type { get; set; }
    
    [Column("brand")]
    public string? Brand { get; set; }
    
    [Column("model")]
    public string? Model { get; set; }
    
    [Column("serial")]
    public string? Serial { get; set; }
    
    [Column("location")]
    public string? Location { get; set; }
    
    [Column("purchase_date")]
    public DateTime? PurchaseDate { get; set; }
    
    [Column("warranty_end")]
    public DateTime? WarrantyEnd { get; set; }
    
    [Column("notes")]
    public string? Notes { get; set; }
    
    [Column("status")]
    public string? Status { get; set; }
}

public class InfoWorkActivity
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("title")]
    public string? Title { get; set; }
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("category")]
    public string? Category { get; set; }
    
    [Column("created_by")]
    public string? CreatedBy { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class InfoWorkAlert
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Column("title")]
    public string? Title { get; set; }
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("priority")]
    public string? Priority { get; set; }
    
    [Column("status")]
    public string? Status { get; set; }
    
    [Column("created_by")]
    public string? CreatedBy { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
