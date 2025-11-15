
namespace FelterAPI.Models;

public class LFChatConversation
{
    public Guid Id { get; set; }
    
    public Guid OrgId { get; set; }
    
    public string? Title { get; set; }
    
    public Guid? StartedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public class LFChatMessage
{
    public Guid Id { get; set; }
    
    public Guid ConversationId { get; set; }
    
    public Guid? SenderId { get; set; }
    
    public string Content { get; set; } = string.Empty;
    
    public string? Type { get; set; }
    
    public bool AiCheck { get; set; }
    
    public string? AiResponse { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
