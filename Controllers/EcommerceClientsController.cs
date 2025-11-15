using System.Text.Json;
using FelterAPI.Data;
using FelterAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FelterAPI.Controllers;

[ApiController]
[Route("api/ecommerce/clients")]
[Authorize]
public class EcommerceClientsController : ControllerBase
{
    private readonly FelterContext _ctx;

    public EcommerceClientsController(FelterContext ctx)
    {
        _ctx = ctx;
    }

    // DTO usado na criação completa de um cliente E-commerce
    public class EcommerceCreateClientRequest
    {
        // Informações do Cliente
        public string CompanyName { get; set; } = string.Empty;
        public string? TradeName { get; set; }
        public string? Cnpj { get; set; }
        public string? Phone { get; set; }
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string? Plan { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? CustomUrl { get; set; }

        // Módulos
        public bool EnableProdutos { get; set; }
        public bool EnableCardapio { get; set; }
        public bool EnableBlog { get; set; }
        public bool EnableVideos { get; set; }
        public bool EnableUsuarios { get; set; }
        public bool EnableConfiguracoes { get; set; }
        public bool EnableAgendamentos { get; set; }
        public bool EnableWhatsapp { get; set; }

        // Firebase
        public string FirebaseApiKey { get; set; } = string.Empty;
        public string FirebaseAuthDomain { get; set; } = string.Empty;
        public string FirebaseProjectId { get; set; } = string.Empty;
        public string? FirebaseStorageBucket { get; set; }
        public string? FirebaseSenderId { get; set; }
        public string? FirebaseAppId { get; set; }
        public string? FirebaseMeasurementId { get; set; }
        public string? FirebaseServiceAccountJson { get; set; }
    }

    // GET: api/ecommerce/clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EcommerceClient>>> GetAll()
    {
        var list = await _ctx.EcommerceClients
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return Ok(list);
    }

    // GET: api/ecommerce/clients/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EcommerceClient>> GetById(Guid id)
    {
        var client = await _ctx.EcommerceClients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null) return NotFound();
        return Ok(client);
    }

    // POST simples (caso queira só criar registro manualmente)
    [HttpPost]
    public async Task<ActionResult<EcommerceClient>> CreateSimple(EcommerceClient client)
    {
        client.Id = client.Id == Guid.Empty ? Guid.NewGuid() : client.Id;
        client.CreatedAt = client.CreatedAt == default ? DateTime.UtcNow : client.CreatedAt;

        _ctx.EcommerceClients.Add(client);
        await _ctx.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
    }

    // POST completo: cria cliente + admin + módulos + db_config
    [HttpPost("create-client")]
    public async Task<ActionResult> CreateClient([FromBody] EcommerceCreateClientRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName) ||
            string.IsNullOrWhiteSpace(request.AdminEmail) ||
            string.IsNullOrWhiteSpace(request.AdminPassword) ||
            string.IsNullOrWhiteSpace(request.Slug))
        {
            return BadRequest("Nome da empresa, email do administrador, senha inicial e slug são obrigatórios.");
        }

        // Verifica se já existe cliente com o mesmo slug
        var slugExists = await _ctx.EcommerceClients
            .AsNoTracking()
            .AnyAsync(c => c.Slug == request.Slug);
        if (slugExists)
            return Conflict("Já existe um cliente com esse slug.");

        // Verifica se já existe usuário com o mesmo email
        var userExists = await _ctx.EcommerceUsers
            .AsNoTracking()
            .AnyAsync(u => u.Email == request.AdminEmail);
        if (userExists)
            return Conflict("Já existe um usuário com esse email.");

        var now = DateTime.UtcNow;
        var clientId = Guid.NewGuid();

        var client = new EcommerceClient
        {
            Id = clientId,
            MasterId = null, // pode ser preenchido depois com o usuário master logado
            Name = request.CompanyName,
            Email = request.AdminEmail,
            Plan = string.IsNullOrWhiteSpace(request.Plan) ? "basic" : request.Plan,
            TradeName = request.TradeName,
            Cnpj = request.Cnpj,
            Phone = request.Phone,
            Slug = request.Slug,
            CustomUrl = request.CustomUrl,
            FirebaseApiKey = request.FirebaseApiKey,
            FirebaseAuthDomain = request.FirebaseAuthDomain,
            FirebaseProjectId = request.FirebaseProjectId,
            FirebaseStorageBucket = request.FirebaseStorageBucket,
            FirebaseSenderId = request.FirebaseSenderId,
            FirebaseAppId = request.FirebaseAppId,
            FirebaseMeasurementId = request.FirebaseMeasurementId,
            FirebaseServiceAccountJson = request.FirebaseServiceAccountJson,
            CreatedAt = now
        };

        var adminUser = new EcommerceUser
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            Name = "Administrador",
            Email = request.AdminEmail,
            Role = "admin",
            Permissions = "all",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.AdminPassword),
            IsActive = true,
            CreatedAt = now
        };

        var modules = new List<EcommerceModule>();

        void AddModule(string key, string name, bool enabled)
        {
            if (!enabled) return;

            modules.Add(new EcommerceModule
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                Key = key,
                Name = name,
                IsEnabled = true,
                CreatedAt = now
            });
        }

        AddModule("produtos",      "Produtos",           request.EnableProdutos);
        AddModule("cardapio",      "Cardápio Online",    request.EnableCardapio);
        AddModule("blog",          "Blog",               request.EnableBlog);
        AddModule("videos",        "Vídeos",             request.EnableVideos);
        AddModule("usuarios",      "Usuários",           request.EnableUsuarios);
        AddModule("configuracoes", "Configurações",      request.EnableConfiguracoes);
        AddModule("agendamentos",  "Agendamentos",       request.EnableAgendamentos);
        AddModule("whatsapp",      "WhatsApp",           request.EnableWhatsapp);

        var firebaseJsonObj = new
        {
            apiKey = request.FirebaseApiKey,
            authDomain = request.FirebaseAuthDomain,
            projectId = request.FirebaseProjectId,
            storageBucket = request.FirebaseStorageBucket,
            messagingSenderId = request.FirebaseSenderId,
            appId = request.FirebaseAppId,
            measurementId = request.FirebaseMeasurementId
        };

        var dbConfig = new EcommerceDbConfig
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            FirebaseJson = JsonSerializer.Serialize(firebaseJsonObj),
            Status = "active",
            CreatedAt = now
        };

        _ctx.EcommerceClients.Add(client);
        _ctx.EcommerceUsers.Add(adminUser);
        if (modules.Count > 0)
            _ctx.EcommerceModules.AddRange(modules);
        _ctx.EcommerceDbConfigs.Add(dbConfig);

        await _ctx.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = clientId }, new
        {
            client.Id,
            client.Name,
            client.Email,
            client.Slug
        });
    }

    // PUT: api/ecommerce/clients/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, EcommerceClient input)
    {
        var existing = await _ctx.EcommerceClients.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Name = input.Name;
        existing.Email = input.Email;
        existing.Plan = input.Plan;
        existing.TradeName = input.TradeName;
        existing.Cnpj = input.Cnpj;
        existing.Phone = input.Phone;
        existing.Slug = input.Slug;
        existing.CustomUrl = input.CustomUrl;
        existing.FirebaseApiKey = input.FirebaseApiKey;
        existing.FirebaseAuthDomain = input.FirebaseAuthDomain;
        existing.FirebaseProjectId = input.FirebaseProjectId;
        existing.FirebaseStorageBucket = input.FirebaseStorageBucket;
        existing.FirebaseSenderId = input.FirebaseSenderId;
        existing.FirebaseAppId = input.FirebaseAppId;
        existing.FirebaseMeasurementId = input.FirebaseMeasurementId;
        existing.FirebaseServiceAccountJson = input.FirebaseServiceAccountJson;

        await _ctx.SaveChangesAsync();
        return Ok(existing);
    }

    // DELETE: api/ecommerce/clients/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _ctx.EcommerceClients.FindAsync(id);
        if (existing == null) return NotFound();

        _ctx.EcommerceClients.Remove(existing);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}
