using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ControleDeEstoque.Models.Common;

namespace ControleDeEstoque.Models
{
    public class Product : Entity
    {
        [MaxLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
        [DisplayName("Nome do Produto")]
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string Name { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior ou igual a zero.")]
        [DisplayName("Preço de Venda")]
        [Required(ErrorMessage = "O preço de venda é obrigatório.")]
        public double SellPrice { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "O preço de compra deve ser maior ou igual a zero.")]
        [DisplayName("Preço de Compra")]
        [Required(ErrorMessage = "O preço de compra é obrigatório.")]
        public double BuyPrice { get; set; }
    }
}
