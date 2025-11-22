using System.Text;
using FelterAPI.Data;
using FelterAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------- CORS ----------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ---------------- Connection String ----------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

builder.Services.AddDbContext<FelterContext>(options =>
    options.UseNpgsql(connectionString));

// DB bootstrap (create schemas/extensions if missing)
builder.Services.AddSingleton<DatabaseBootstrapper>();

builder.Services.AddScoped<FirebaseDynamicService>();

// ---------------- Controllers + Swagger ----------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Felter Technology Ecosystem API",
        Version = "v1",
        Description = "Master + Clientes (StockFlow, InfoWork, OmniFlow, LFChat, E-commerce)"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticação JWT. Exemplo: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ---------------- JWT ----------------
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Ensure required schemas/extensions exist (safe no-op if already present)
using (var scope = app.Services.CreateScope())
using Microsoft.Extensions.DependencyInjection;
{
    var bootstrapper = scope.ServiceProvider.GetRequiredService<DatabaseBootstrapper>();
    await bootstrapper.EnsureSchemasAndExtensionsAsync();
}


// ---------------- OPTIONS (antes de tudo) ----------------
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Append("Access-Control-Allow-Headers", "*");
        context.Response.Headers.Append("Access-Control-Allow-Methods", "*");
        context.Response.StatusCode = 200;
        return;
    }

    await next();
});

// ---------------- CORS (antes de Auth, Swagger e Controllers) ----------------
app.UseCors("AllowAll");

// ---------------- Proteção básica do Swagger ----------------
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        string? authHeader = context.Request.Headers["Authorization"];

        if (authHeader != null && authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            var encodedUsernamePassword = authHeader["Basic ".Length..].Trim();
            var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
            var parts = decodedUsernamePassword.Split(':', 2);
            var username = parts[0];
            var password = parts.Length > 1 ? parts[1] : string.Empty;

            if (username == "dev.feltertechnology@gmail.com" && password == "#MjlEgdc123451958#")
            {
                await next();
                return;
            }
        }

        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Felter Swagger\"";
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("🚫 Acesso não autorizado ao Swagger.");
        return;
    }

    await next();
});

// ---------------- Swagger ----------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Felter Ecosystem v1");
    c.RoutePrefix = string.Empty;
});

// ---------------- Auth ----------------
app.UseAuthentication();
app.UseAuthorization();

// ---------------- Controllers ----------------
app.MapControllers();

app.Run();