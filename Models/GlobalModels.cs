using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models;

public class GlobalUser
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = default!;
    
    [Column("email")]
    public string Email { get; set; } = default!;
    
    [Column("password_hash")]
    public string PasswordHash { get; set; } = default!;
    
    [Column("is_super_admin")]
    public bool IsSuperAdmin { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class Organization
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = default!;
    
    [Column("email")]
    public string? Email { get; set; }
    
    [Column("phone")]
    public string? Phone { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class UserOrgRole
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("org_id")]
    public Guid OrgId { get; set; }
    
    [Column("role")]
    public string Role { get; set; } = "user";
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}


public class OrgModule
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("org_id")]
    public Guid OrgId { get; set; }
    
    [Column("module_key")]
    public string ModuleKey { get; set; } = default!;
    
    [Column("is_enabled")]
    public bool IsEnabled { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}