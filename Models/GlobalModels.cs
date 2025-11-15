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
    
    [Column("passwordhash")]
    public string PasswordHash { get; set; } = default!;
    
    [Column("issuperadmin")]
    public bool IsSuperAdmin { get; set; }
    
    [Column("createdat")]
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
    
    [Column("isactive")]
    public bool IsActive { get; set; } = true;
    
    [Column("createdat")]
    public DateTime CreatedAt { get; set; }
}

public class UserOrgRole
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("userid")]
    public Guid UserId { get; set; }
    
    [Column("orgid")]
    public Guid OrgId { get; set; }
    
    [Column("role")]
    public string Role { get; set; } = "user";
    
    [Column("isactive")]
    public bool IsActive { get; set; } = true;
    
    [Column("createdat")]
    public DateTime CreatedAt { get; set; }
}


public class OrgModule
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("orgid")]
    public Guid OrgId { get; set; }
    
    [Column("modulekey")]
    public string ModuleKey { get; set; } = default!;
    
    [Column("isenabled")]
    public bool IsEnabled { get; set; } = true;
    
    [Column("createdat")]
    public DateTime CreatedAt { get; set; }
}