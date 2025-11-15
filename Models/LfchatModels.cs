using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models;

public class LFChatConversation
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("org_id")]
    public Guid OrgId { get; set; }
    
    [Column("title")]
    public string? Title { get; set; }
    
    [Column("started_by")]
    public Guid? StartedBy { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class LFChatMessage
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("conversation_id")]
    public Guid ConversationId { get; set; }
    
    [Column("sender_id")]
    public Guid? SenderId { get; set; }
    
    [Column("content")]
    public string Content { get; set; } = string.Empty;
    
    [Column("type")]
    public string? Type { get; set; }
    
    [Column("ai_check")]
    public bool AiCheck { get; set; }
    
    [Column("ai_response")]
    public string? AiResponse { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
