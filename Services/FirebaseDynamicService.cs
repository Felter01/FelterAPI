
using FelterAPI.Data;
using FelterAPI.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.EntityFrameworkCore;

namespace FelterAPI.Services;

public class FirebaseDynamicService
{
    private readonly FelterContext _ctx;
    private readonly ILogger<FirebaseDynamicService> _logger;

    public FirebaseDynamicService(FelterContext ctx, ILogger<FirebaseDynamicService> logger)
    {
        _ctx = ctx;
        _logger = logger;

        try
        {
            _logger.LogInformation("🔥 FirebaseDynamicService inicializado.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro inicializando FirebaseDynamicService: " + ex);
        }
    }

    private async Task<(EcommerceClient client, EcommerceDbConfig config)?> GetClientConfigAsync(Guid clientId)
    {
        try
        {
            var client = await _ctx.EcommerceClients.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
            {
                _logger.LogWarning("EcommerceClient não encontrado: {ClientId}", clientId);
                return null;
            }

            var cfg = await _ctx.EcommerceDbConfigs.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (cfg == null)
            {
                _logger.LogWarning("EcommerceDbConfig não encontrado para clientId {ClientId}", clientId);
                return null;
            }

            return (client, cfg);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar config do cliente {ClientId}", clientId);
            return null;
        }
    }

    public async Task<FirestoreDb?> GetFirestoreForClientAsync(Guid clientId)
    {
        try
        {
            var result = await GetClientConfigAsync(clientId);
            if (result == null)
                return null;

            var (client, cfg) = result.Value;

            GoogleCredential credential;

            if (!string.IsNullOrWhiteSpace(cfg.FirebaseJson))
            {
                try
                {
                    credential = GoogleCredential.FromJson(cfg.FirebaseJson);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "FirebaseJson inválido para clientId {ClientId}", clientId);
                    return null;
                }
            }
            else
            {
                _logger.LogWarning("FirebaseJson vazio para clientId {ClientId}. Usando credencial padrão.", clientId);
                credential = await GoogleCredential.GetApplicationDefaultAsync();
            }

            var firestoreClient = await new FirestoreClientBuilder
            {
                Credential = credential
            }.BuildAsync();

            if (string.IsNullOrWhiteSpace(client.FirebaseProjectId))
            {
                _logger.LogError("FirebaseProjectId vazio para clientId {ClientId}", clientId);
                return null;
            }

            var db = FirestoreDb.Create(client.FirebaseProjectId, firestoreClient);
            return db;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar FirestoreDb para Client {ClientId}", clientId);
            return null;
        }
    }

    public async Task EnsureBaseCollectionsAsync(Guid clientId)
    {
        var db = await GetFirestoreForClientAsync(clientId);
        if (db == null)
        {
            _logger.LogWarning("Firestore nulo para clientId {ClientId}", clientId);
            return;
        }

        string[] baseCollections =
        {
            "_metadata", "agendamentos", "blog",
            "cardapio_online", "configuracao", "produtos",
            "usuarios", "videos", "whatsapp"
        };

        foreach (var col in baseCollections)
        {
            try
            {
                var docRef = db.Collection(col).Document("_init");
                var snap = await docRef.GetSnapshotAsync();

                if (!snap.Exists)
                {
                    await docRef.SetAsync(new
                    {
                        createdAt = DateTime.UtcNow,
                        system = "Felter E-Commerce"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro criando coleção {Collection} para clientId {ClientId}", col, clientId);
            }
        }
    }

    public async Task<bool> UpsertDocumentAsync(Guid clientId, string collection, string documentId, IDictionary<string, object> data)
    {
        var db = await GetFirestoreForClientAsync(clientId);
        if (db == null)
            return false;

        try
        {
            await db.Collection(collection).Document(documentId).SetAsync(data);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar documento {Doc} em {Col} para {ClientId}", documentId, collection, clientId);
            return false;
        }
    }

    public async Task<IReadOnlyList<Dictionary<string, object>>> GetAllDocumentsAsync(Guid clientId, string collection)
    {
        var db = await GetFirestoreForClientAsync(clientId);
        if (db == null)
            return Array.Empty<Dictionary<string, object>>();

        try
        {
            var snap = await db.Collection(collection).GetSnapshotAsync();
            return snap.Documents.Select(d =>
            {
                var dict = d.ToDictionary();
                dict["__id"] = d.Id;
                return dict;
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro lendo {Collection} para {ClientId}", collection, clientId);
            return Array.Empty<Dictionary<string, object>>();
        }
    }
}
