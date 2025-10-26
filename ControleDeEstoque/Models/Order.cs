using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ControleDeEstoque.Models.Common;

namespace ControleDeEstoque.Models
{
    public class Order:Entity
    {
        [DisplayName("Fornecedor")]
        public Guid SupplierId { get; set; }
        [DisplayName("Fornecedor")]
        public Supplier? Supplier { get; set; }
        [DisplayName("Produto Do Estoque")]
        public Guid StockId { get; set; }
        [DisplayName("Produto Do Estoque")]
        public Stock? Stock { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
        [DisplayName("Quantidade")]
        public int Amount { get; set; }
    }
}
