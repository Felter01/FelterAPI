using FelterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FelterAPI.Data;

public class FelterContext : DbContext
{
    public FelterContext(DbContextOptions<FelterContext> options) : base(options) { }

    // Global
    public DbSet<GlobalUser> GlobalUsers => Set<GlobalUser>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<UserOrgRole> UserOrgRoles => Set<UserOrgRole>();
    public DbSet<GlobalModule> GlobalModules => Set<GlobalModule>();
    public DbSet<OrgModule> OrgModules => Set<OrgModule>();

    // StockFlow
    public DbSet<StockFlowClient> StockFlowClients => Set<StockFlowClient>();
    public DbSet<StockFlowEmployee> StockFlowEmployees => Set<StockFlowEmployee>();
    public DbSet<StockFlowProduct> StockFlowProducts => Set<StockFlowProduct>();
    public DbSet<StockFlowRequest> StockFlowRequests => Set<StockFlowRequest>();
    public DbSet<StockFlowQuotation> StockFlowQuotations => Set<StockFlowQuotation>();
    public DbSet<StockFlowSetting> StockFlowSettings => Set<StockFlowSetting>();

    // InfoWork
    public DbSet<InfoWorkClient> InfoWorkClients => Set<InfoWorkClient>();
    public DbSet<InfoWorkComputer> InfoWorkComputers => Set<InfoWorkComputer>();
    public DbSet<InfoWorkPassword> InfoWorkPasswords => Set<InfoWorkPassword>();
    public DbSet<InfoWorkRouter> InfoWorkRouters => Set<InfoWorkRouter>();
    public DbSet<InfoWorkEquipment> InfoWorkEquipments => Set<InfoWorkEquipment>();
    public DbSet<InfoWorkActivity> InfoWorkActivities => Set<InfoWorkActivity>();
    public DbSet<InfoWorkAlert> InfoWorkAlerts => Set<InfoWorkAlert>();

    // OmniFlow
    public DbSet<OmniFlowClient> OmniFlowClients => Set<OmniFlowClient>();
    public DbSet<OmniFlowFacility> OmniFlowFacilities => Set<OmniFlowFacility>();
    public DbSet<OmniFlowSector> OmniFlowSectors => Set<OmniFlowSector>();
    public DbSet<OmniFlowPerson> OmniFlowPersons => Set<OmniFlowPerson>();
    public DbSet<OmniFlowFaceTemplate> OmniFlowFaceTemplates => Set<OmniFlowFaceTemplate>();
    public DbSet<OmniFlowAccessEvent> OmniFlowAccessEvents => Set<OmniFlowAccessEvent>();

    // Ecommerce
    public DbSet<EcommerceClient> EcommerceClients => Set<EcommerceClient>();
    public DbSet<EcommerceUser> EcommerceUsers => Set<EcommerceUser>();
    public DbSet<EcommerceModule> EcommerceModules => Set<EcommerceModule>();
    public DbSet<EcommerceDbConfig> EcommerceDbConfigs => Set<EcommerceDbConfig>();

    // LFChat
    public DbSet<LFChatConversation> LFChatConversations => Set<LFChatConversation>();
    public DbSet<LFChatMessage> LFChatMessages => Set<LFChatMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar todas as propriedades para usar aspas duplas (case-sensitive)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                property.SetColumnName($"\"{property.GetColumnName()}\"");
            }
        }

        // Schema Global
        modelBuilder.Entity<GlobalUser>().ToTable("users", "schema_global");
        modelBuilder.Entity<Organization>().ToTable("organizations", "schema_global");
        modelBuilder.Entity<UserOrgRole>().ToTable("user_org_roles", "schema_global");
        modelBuilder.Entity<GlobalModule>().ToTable("modules", "schema_global").HasKey(m => m.Key);
        modelBuilder.Entity<OrgModule>().ToTable("org_modules", "schema_global");

        // StockFlow
        modelBuilder.Entity<StockFlowClient>().ToTable("clients", "stockflow");
        modelBuilder.Entity<StockFlowEmployee>().ToTable("employees", "stockflow");
        modelBuilder.Entity<StockFlowProduct>().ToTable("products", "stockflow");
        modelBuilder.Entity<StockFlowRequest>().ToTable("requests", "stockflow");
        modelBuilder.Entity<StockFlowQuotation>().ToTable("quotations", "stockflow");
        modelBuilder.Entity<StockFlowSetting>().ToTable("settings", "stockflow");

        // InfoWork
        modelBuilder.Entity<InfoWorkClient>().ToTable("clients", "infowork");
        modelBuilder.Entity<InfoWorkComputer>().ToTable("computers", "infowork");
        modelBuilder.Entity<InfoWorkPassword>().ToTable("passwords", "infowork");
        modelBuilder.Entity<InfoWorkRouter>().ToTable("routers", "infowork");
        modelBuilder.Entity<InfoWorkEquipment>().ToTable("equipments", "infowork");
        modelBuilder.Entity<InfoWorkActivity>().ToTable("activities", "infowork");
        modelBuilder.Entity<InfoWorkAlert>().ToTable("alerts", "infowork");

        // OmniFlow
        modelBuilder.Entity<OmniFlowClient>().ToTable("clients", "ominiflow");
        modelBuilder.Entity<OmniFlowFacility>().ToTable("facilities", "ominiflow");
        modelBuilder.Entity<OmniFlowSector>().ToTable("sectors", "ominiflow");
        modelBuilder.Entity<OmniFlowPerson>().ToTable("persons", "ominiflow");
        modelBuilder.Entity<OmniFlowFaceTemplate>().ToTable("face_templates", "ominiflow");
        modelBuilder.Entity<OmniFlowAccessEvent>().ToTable("access_events", "ominiflow");

        // Ecommerce
        modelBuilder.Entity<EcommerceClient>().ToTable("clients", "ecommerce_felter");
        modelBuilder.Entity<EcommerceUser>().ToTable("users", "ecommerce_felter");
        modelBuilder.Entity<EcommerceModule>().ToTable("modules", "ecommerce_felter");
        modelBuilder.Entity<EcommerceDbConfig>().ToTable("db_config", "ecommerce_felter");

        // LFChat
        modelBuilder.Entity<LFChatConversation>().ToTable("conversations", "lfchat");
        modelBuilder.Entity<LFChatMessage>().ToTable("messages", "lfchat");
    }
}
