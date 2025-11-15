using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models;

public class EcommerceClient
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("masterid")]
    public Guid? MasterId { get; set; }

    // Dados básicos
    [Column("name")]
    public string? Name { get; set; }

    // Login do cliente (acesso ao dashboard do e-commerce)
    [Column("email")]
    public string? Email { get; set; }
    
    [Column("password")]
    public string? Password { get; set; }
    
    [Column("isactive")]
    public bool IsActive { get; set; } = true;

    // Firebase do e-commerce do cliente
    [Column("plan")]
    public string? Plan { get; set; }
    
    [Column("firebaseapikey")]
    public string? FirebaseApiKey { get; set; }
    
    [Column("firebaseauthdomain")]
    public string? FirebaseAuthDomain { get; set; }
    
    [Column("firebaseprojectid")]
    public string? FirebaseProjectId { get; set; }
    
    [Column("firebasestoragebucket")]
    public string? FirebaseStorageBucket { get; set; }
    
    [Column("firebasesenderid")]
    public string? FirebaseSenderId { get; set; }
    
    [Column("firebaseappid")]
    public string? FirebaseAppId { get; set; }

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }
}


public class EcommerceUser
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("clientid")]
    public Guid ClientId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("email")]
    public string? Email { get; set; }
    
    [Column("passwordhash")]
    public string? PasswordHash { get; set; }
    
    [Column("role")]
    public string? Role { get; set; }
    
    [Column("permissions")]
    public string? Permissions { get; set; } // JSON string
    
    [Column("isactive")]
    public bool IsActive { get; set; } = true;
    
    [Column("createdat")]
    public DateTime CreatedAt { get; set; }
}

public class EcommerceModule
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("clientid")]
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
    
    [Column("clientid")]
    public Guid ClientId { get; set; }
    
    [Column("firebasejson")]
    public string? FirebaseJson { get; set; }
    
    [Column("status")]
    public string? Status { get; set; }
}
