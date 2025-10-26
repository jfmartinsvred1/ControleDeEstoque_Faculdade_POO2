using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ControleDeEstoque.Models.Common;

namespace ControleDeEstoque.Models
{
    public class Supplier:Entity
    {
        [DisplayName("Nome do Fornecedor")]
        [Required(ErrorMessage = "O nome do fornecedor é obrigatório.")]
        public string Name { get; set; }
        [DisplayName("Numero de Telefone")]
        [Required(ErrorMessage = "O número de telefone é obrigatório.")]
        [Phone(ErrorMessage = "O número de telefone não é válido.")]
        public string Phone { get; set; }
        [DisplayName("Endereço")]
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Address { get; set; }
    }
}
