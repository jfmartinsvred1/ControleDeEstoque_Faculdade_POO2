namespace ControleDeEstoque.Models
{
    public class TotalBySupplier
    {
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public double OrderValue { get; set; }
    }
}
