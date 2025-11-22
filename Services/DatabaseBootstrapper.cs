
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace FelterAPI.Services;

public class DatabaseBootstrapper
{
    private readonly IConfiguration _config;
    private readonly ILogger<DatabaseBootstrapper> _logger;

    public DatabaseBootstrapper(IConfiguration config, ILogger<DatabaseBootstrapper> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task EnsureSchemasAndExtensionsAsync()
    {
        var connString = _config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connString))
        {
            _logger.LogWarning("DefaultConnection not configured; skipping DB bootstrap.");
            return;
        }

        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();

        string[] schemas =
        {
            "schema_global",
            "ecommerce_felter",
            "lfchat",
            "stockflow",
            "infowork",
            "ominiflow"
        };

        foreach (var schema in schemas)
        {
            var sql = $"CREATE SCHEMA IF NOT EXISTS {schema};";
            await using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        // Safe extensions used by your DB/functions
        string[] extensions = { "pg_trgm", "pgcrypto", "uuid-ossp" };
        foreach (var ext in extensions)
        {
            var sql = $"CREATE EXTENSION IF NOT EXISTS \"{ext}\";";
            await using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        _logger.LogInformation("DB bootstrap finished: schemas/extensions ensured.");
    }
}
