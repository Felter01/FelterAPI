namespace FelterAPI.Models;

public class GlobalUser
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool IsSuperAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

public class UserOrgRole
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OrgId { get; set; }
    public string Role { get; set; } = "user";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

public class GlobalModule
{
    public string Key { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class OrgModule
{
    public Guid Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModuleKey { get; set; } = default!;
    public bool IsEnabled { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
