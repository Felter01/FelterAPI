
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace FelterAPI.Models;

public class EcommerceUser
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

    [Column("permissions", TypeName="jsonb")]
    public JsonDocument? Permissions { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
