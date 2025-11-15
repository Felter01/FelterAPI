namespace FelterAPI.Models;

public class EcommerceClient
{
    public Guid Id { get; set; }
    public Guid? MasterId { get; set; }

    // Dados básicos
    public string? Name { get; set; }

    // Login do cliente (acesso ao dashboard do e-commerce)
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool IsActive { get; set; } = true;

    // Firebase do e-commerce do cliente
    public string? Plan { get; set; }
    public string? FirebaseApiKey { get; set; }
    public string? FirebaseAuthDomain { get; set; }
    public string? FirebaseProjectId { get; set; }
    public string? FirebaseStorageBucket { get; set; }
    public string? FirebaseSenderId { get; set; }
    public string? FirebaseAppId { get; set; }

    public DateTime CreatedAt { get; set; }
}


public class EcommerceUser
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Role { get; set; }
    public string? Permissions { get; set; } // JSON string
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

public class EcommerceModule
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string? Key { get; set; }
    public bool Enabled { get; set; } = true;
}

public class EcommerceDbConfig
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string? FirebaseJson { get; set; }
    public string? Status { get; set; }
}
