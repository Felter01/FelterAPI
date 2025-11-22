using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models;

// ==========================
// ECOMMERCE - PADRÃO SNAKE_CASE
// Tabelas no schema ecommerce_felter:
// clients, users, modules, db_config
// ==========================

public class EcommerceClient
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("master_id")]
    public Guid? MasterId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("plan")]
    public string? Plan { get; set; }

    [Column("trade_name")]
    public string? TradeName { get; set; }

    [Column("cnpj")]
    public string? Cnpj { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("slug")]
    public string? Slug { get; set; }

    [Column("custom_url")]
    public string? CustomUrl { get; set; }

    [Column("firebase_api_key")]
    public string? FirebaseApiKey { get; set; }

    [Column("firebase_auth_domain")]
    public string? FirebaseAuthDomain { get; set; }

    [Column("firebase_project_id")]
    public string? FirebaseProjectId { get; set; }

    [Column("firebase_storage_bucket")]
    public string? FirebaseStorageBucket { get; set; }

    [Column("firebase_sender_id")]
    public string? FirebaseSenderId { get; set; }

    [Column("firebase_app_id")]
    public string? FirebaseAppId { get; set; }

    [Column("firebase_measurement_id")]
    public string? FirebaseMeasurementId { get; set; }

    [Column("firebase_service_account_json")]
    public string? FirebaseServiceAccountJson { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

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

    [Column("permissions")]
    public string? Permissions { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class EcommerceModule
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("client_id")]
    public Guid ClientId { get; set; }

    [Column("key")]
    public string? Key { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    // Mantemos apenas a coluna oficial
    [Column("is_enabled")]
    public bool IsEnabled { get; set; } = true;

    // Campo legado, não mapeado
    [NotMapped]
    public bool Enabled { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class EcommerceDbConfig
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("client_id")]
    public Guid ClientId { get; set; }

    [Column("firebase_json")]
    public string? FirebaseJson { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
