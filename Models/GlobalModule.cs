using System.ComponentModel.DataAnnotations.Schema;

namespace FelterAPI.Models
{
    public class GlobalModule
    {
        /// <summary>
        /// Chave única do módulo (PRIMARY KEY)
        /// Ex: "produtos", "blog", "cardapio"
        /// </summary>
        [Column("key")]
        public string Key { get; set; } = default!;

        /// Nome exibido no painel
        [Column("name")]
        public string Name { get; set; } = default!;

        /// Descrição amigável
        [Column("description")]
        public string Description { get; set; } = default!;

        /// Se está ativo globalmente
        [Column("active")]
        public bool Active { get; set; } = true;

        /// Data de criação
        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// Data de atualização
        [Column("updatedat")]
        public DateTime? UpdatedAt { get; set; }
    }
}
