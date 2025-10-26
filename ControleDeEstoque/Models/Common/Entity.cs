using System.ComponentModel;

namespace ControleDeEstoque.Models.Common
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DisplayName("Data de Criação")]
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        [DisplayName("Data de Atualização")]
        public DateTime? UpdatedAt { get; set; }
    }
}
