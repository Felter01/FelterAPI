using System.Text.Json;
using FelterAPI.Data;
using FelterAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FelterAPI.Controllers;

[ApiController]
[Route("api/ecommerce/clients")]
[Authorize] // pode remover se quiser liberar publicamente
public class EcommerceClientsController : ControllerBase
{
    private readonly FelterContext _ctx;
    private readonly ILogger<EcommerceClientsController> _logger;

    public EcommerceClientsController(FelterContext ctx, ILogger<EcommerceClientsController> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    // DTO de criação
    public class EcommerceCreateClientRequest
    {
        public string CompanyName { get; set; } = string.Empty;
        public string? TradeName { get; set; }
        public string? Cnpj { get; set; }
        public string? Phone { get; set; }
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string? Plan { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? CustomUrl { get; set; }

        public bool EnableProdutos { get; set; }
        public bool EnableCardapio { get; set; }
        public bool EnableBlog { get; set; }
        public bool EnableVideos { get; set; }
        public bool EnableUsuarios { get; set; }
        public bool EnableConfiguracoes { get; set; }
        public bool EnableAgendamentos { get; set; }
        public bool EnableWhatsapp { get; set; }

        public string FirebaseApiKey { get; set; } = string.Empty;
        public string FirebaseAuthDomain { get; set; } = string.Empty;
        public string FirebaseProjectId { get; set; } = string.Empty;
        public string? FirebaseStorageBucket { get; set; }
        public string? FirebaseSenderId { get; set; }
        public string? FirebaseAppId { get; set; }
        public string? FirebaseMeasurementId { get; set; }
        public string? FirebaseServiceAccountJson { get; set; }
    }

    // GET
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var list = await _ctx.EcommerceClients
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return Ok(list);
    }

    // GET by ID
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var client = await _ctx.EcommerceClients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (client == null) return NotFound();

        return Ok(client);
    }

    // POST COMPLETO
    [HttpPost("create-client")]
    public async Task<ActionResult> CreateClient([FromBody] EcommerceCreateClientRequest request)
    {
        try
        {
            // Validação inicial
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.CompanyName) ||
                string.IsNullOrWhiteSpace(request.AdminEmail) ||
                string.IsNullOrWhiteSpace(request.AdminPassword) ||
                string.IsNullOrWhiteSpace(request.Slug))
            {
                return BadRequest(new { error = "CompanyName, AdminEmail, AdminPassword e Slug são obrigatórios." });
            }

            // Slug duplicado
            if (await _ctx.EcommerceClients.AsNoTracking().AnyAsync(c => c.Slug == request.Slug))
                return Conflict(new { error = "Slug já está em uso." });

            // Email duplicado
            if (await _ctx.EcommerceUsers.AsNoTracking().AnyAsync(u => u.Email == request.AdminEmail))
                return Conflict(new { error = "Já existe um usuário com este email." });

            var now = DateTime.UtcNow;
            var clientId = Guid.NewGuid();

            // Monta o cliente
            var client = new EcommerceClient
            {
                Id = clientId,
                MasterId = null,
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

            // Admin user
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

            // Módulos
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

            AddModule("produtos", "Produtos", request.EnableProdutos);
            AddModule("cardapio", "Cardápio Online", request.EnableCardapio);
            AddModule("blog", "Blog", request.EnableBlog);
            AddModule("videos", "Vídeos", request.EnableVideos);
            AddModule("usuarios", "Usuários", request.EnableUsuarios);
            AddModule("configuracoes", "Configurações", request.EnableConfiguracoes);
            AddModule("agendamentos", "Agendamentos", request.EnableAgendamentos);
            AddModule("whatsapp", "WhatsApp", request.EnableWhatsapp);

            // Firebase JSON
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
    FirebaseJson = JsonDocument.Parse(JsonSerializer.Serialize(firebaseJsonObj)),
    Status = "active",
    CreatedAt = now
};


            // Salvar no banco
            _ctx.EcommerceClients.Add(client);
            _ctx.EcommerceUsers.Add(adminUser);
            if (modules.Count > 0)
                _ctx.EcommerceModules.AddRange(modules);
            _ctx.EcommerceDbConfigs.Add(dbConfig);

            await _ctx.SaveChangesAsync();

            return Ok(new
            {
                message = "Cliente criado com sucesso!",
                clientId = client.Id,
                client.Name,
                client.Email,
                client.Slug
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cliente.");
            return StatusCode(500, new { error = "Erro interno ao criar cliente.", details = ex.Message });
        }
    }

    // UPDATE
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, EcommerceClient input)
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

    // DELETE
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var existing = await _ctx.EcommerceClients.FindAsync(id);
        if (existing == null) return NotFound();

        _ctx.EcommerceClients.Remove(existing);
        await _ctx.SaveChangesAsync();

        return NoContent();
    }
}
