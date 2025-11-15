
using System.Text.Json;
using FelterAPI.Data;
using FelterAPI.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.EntityFrameworkCore;

namespace FelterAPI.Services;

/// <summary>
/// Serviço responsável por conectar dinamicamente no Firestore de cada cliente de e-commerce,
/// usando as credenciais salvas no banco PostgreSQL (EcommerceClient / EcommerceDbConfig).
/// </summary>
public class FirebaseDynamicService
{
    private readonly FelterContext _ctx;
    private readonly ILogger<FirebaseDynamicService> _logger;

    // Coleções base que o e-commerce do cliente precisa ter no Firestore
    private static readonly string[] BaseCollections = new[]
    {
        "_metadata",
        "agendamentos",
        "blog",
        "cardapio_online",
        "configuracao",
        "produtos",
        "usuarios",
        "videos",
        "whatsapp"
    };

    public FirebaseDynamicService(FelterContext ctx, ILogger<FirebaseDynamicService> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    /// <summary>
    /// Retorna a configuração de banco Firebase (EcommerceDbConfig) + dados do cliente.
    /// </summary>
    private async Task<(EcommerceClient client, EcommerceDbConfig config)?> GetClientConfigAsync(Guid clientId, CancellationToken ct = default)
    {
        var client = await _ctx.EcommerceClients.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == clientId, ct);

        if (client == null)
        {
            _logger.LogWarning("EcommerceClient não encontrado para ClientId {ClientId}", clientId);
            return null;
        }

        var cfg = await _ctx.EcommerceDbConfigs.AsNoTracking()
            .FirstOrDefaultAsync(c => c.ClientId == clientId, ct);

        if (cfg == null)
        {
            _logger.LogWarning("EcommerceDbConfig não encontrado para ClientId {ClientId}", clientId);
            return null;
        }

        if (string.IsNullOrWhiteSpace(client.FirebaseProjectId))
        {
            _logger.LogWarning("Cliente {ClientId} não possui FirebaseProjectId configurado.", clientId);
            return null;
        }

        return (client, cfg);
    }

    /// <summary>
    /// Cria uma instância de FirestoreDb para o cliente informado, baseada nas credenciais salvas.
    ///
    /// ⚠ IMPORTANTE:
    /// - Se EcommerceDbConfig.FirebaseJson estiver preenchido, ele será tratado como JSON de service account.
    /// - Caso contrário, o código tentará usar GoogleCredential.GetApplicationDefault()
    ///   (é necessário configurar a variável de ambiente GOOGLE_APPLICATION_CREDENTIALS no servidor).
    /// </summary>
    public async Task<FirestoreDb?> GetFirestoreForClientAsync(Guid clientId, CancellationToken ct = default)
    {
        var info = await GetClientConfigAsync(clientId, ct);
        if (info == null) return null;

        var (client, cfg) = info.Value;

        try
        {
            GoogleCredential credential;

            if (!string.IsNullOrWhiteSpace(cfg.FirebaseJson))
            {
                // FirebaseJson deve conter o JSON completo da service account do projeto Firebase do cliente
                credential = GoogleCredential.FromJson(cfg.FirebaseJson);
            }
            else
            {
                // Fallback: usa credenciais padrão do ambiente
                credential = await GoogleCredential.GetApplicationDefaultAsync(ct);
            }

            var firestoreBuilder = new FirestoreClientBuilder
            {
                Credential = credential
            };

            var firestoreClient = await firestoreBuilder.BuildAsync(ct);

            var db = FirestoreDb.Create(client.FirebaseProjectId!, firestoreClient);
            _logger.LogInformation("Conexão Firestore criada para cliente {ClientId} (ProjectId={ProjectId})", clientId, client.FirebaseProjectId);

            return db;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar FirestoreDb para cliente {ClientId}", clientId);
            return null;
        }
    }

    /// <summary>
    /// Garante que todas as coleções base do e-commerce existam no Firestore do cliente,
    /// criando um documento '_init' em cada uma delas, se ainda não existir.
    /// </summary>
    public async Task EnsureBaseCollectionsAsync(Guid clientId, CancellationToken ct = default)
    {
        var db = await GetFirestoreForClientAsync(clientId, ct);
        if (db == null)
        {
            _logger.LogWarning("Não foi possível garantir coleções para ClientId {ClientId} porque o FirestoreDb é nulo.", clientId);
            return;
        }

        foreach (var col in BaseCollections)
        {
            var docRef = db.Collection(col).Document("_init");

            try
            {
                var snapshot = await docRef.GetSnapshotAsync(ct);
                if (!snapshot.Exists)
                {
                    var initData = new Dictionary<string, object>
                    {
                        ["createdAt"] = DateTime.UtcNow,
                        ["system"] = "Felter E-commerce",
                        ["note"] = "Documento inicial criado automaticamente pelo FirebaseDynamicService."
                    };

                    await docRef.SetAsync(initData, cancellationToken: ct);
                    _logger.LogInformation("Coleção {Collection} inicializada para ClientId {ClientId}.", col, clientId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inicializar coleção {Collection} para ClientId {ClientId}.", col, clientId);
            }
        }
    }

    /// <summary>
    /// Helper genérico para gravar (criar/atualizar) um documento no Firestore do cliente.
    /// Pode ser usado, por exemplo, para produtos, usuários, agendamentos etc.
    /// </summary>
    public async Task<bool> UpsertDocumentAsync(
        Guid clientId,
        string collectionName,
        string documentId,
        IDictionary<string, object> data,
        CancellationToken ct = default)
    {
        var db = await GetFirestoreForClientAsync(clientId, ct);
        if (db == null) return false;

        try
        {
            var docRef = db.Collection(collectionName).Document(documentId);
            await docRef.SetAsync(data, cancellationToken: ct);
            _logger.LogInformation("Documento {DocumentId} salvo em {Collection} para ClientId {ClientId}.", documentId, collectionName, clientId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar documento {DocumentId} em {Collection} para ClientId {ClientId}.", documentId, collectionName, clientId);
            return false;
        }
    }

    /// <summary>
    /// Helper para buscar todos os documentos de uma coleção do cliente (sem filtros).
    /// Pode ser melhorado depois com filtros, paginação etc.
    /// </summary>
    public async Task<IReadOnlyList<Dictionary<string, object>>> GetAllDocumentsAsync(
        Guid clientId,
        string collectionName,
        CancellationToken ct = default)
    {
        var db = await GetFirestoreForClientAsync(clientId, ct);
        if (db == null) return Array.Empty<Dictionary<string, object>>();

        try
        {
            var query = db.Collection(collectionName);
            var snapshot = await query.GetSnapshotAsync(ct);

            var list = new List<Dictionary<string, object>>();

            foreach (var doc in snapshot.Documents)
            {
                var dict = doc.ToDictionary();
                dict["__id"] = doc.Id;
                list.Add(dict);
            }

            _logger.LogInformation("Carregados {Count} documentos de {Collection} para ClientId {ClientId}.", list.Count, collectionName, clientId);
            return list;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar documentos de {Collection} para ClientId {ClientId}.", collectionName, clientId);
            return Array.Empty<Dictionary<string, object>>();
        }
    }
}
