using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ControleDeEstoque.Models.Common;

namespace ControleDeEstoque.Models
{
    public class Stock:Entity
    {
        [DisplayName("Quantidade")]
        public int? Amount { get; set; }
        [Required(ErrorMessage = "O produto é obrigatório!")]
        [DisplayName("Produto")]
        public Guid ProductId { get; set; }
        [DisplayName("Produto")]
        public Product? Product { get; set; }
    }
}
