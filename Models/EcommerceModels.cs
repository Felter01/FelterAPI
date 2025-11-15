using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models;

public class EcommerceClient
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("master_id")]
    public Guid? MasterId { get; set; }

    // Dados básicos
    [Column("name")]
    public string? Name { get; set; }

    // Login do cliente (acesso ao dashboard do e-commerce)
    [Column("email")]
    public string? Email { get; set; }
    
    [Column("password")]
    public string? Password { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Firebase do e-commerce do cliente
    [Column("plan")]
    public string? Plan { get; set; }
    
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
    public string? Permissions { get; set; } // JSON string
    
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
    
    [Column("enabled")]
    public bool Enabled { get; set; } = true;
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
}
