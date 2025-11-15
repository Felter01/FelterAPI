namespace FelterAPI.Models
{
    public class GlobalModule
    {
        /// <summary>
        /// Chave única do módulo (PRIMARY KEY)
        /// Ex: "produtos", "blog", "cardapio"
        /// </summary>
        public string Key { get; set; }

        /// Nome exibido no painel
        public string DisplayName { get; set; }

        /// Descrição amigável
        public string Description { get; set; }

        /// Se está ativo globalmente
        public bool Active { get; set; } = true;

        /// Data de criação
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// Data de atualização
        public DateTime? UpdatedAt { get; set; }
    }
}
