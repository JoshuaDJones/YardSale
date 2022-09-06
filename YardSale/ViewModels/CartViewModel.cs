using Core.YardSale.Contracts;

namespace YardSale.ViewModels
{
    public class CartViewModel
    {
        public List<Product> CartProducts { get; set; } = new List<Product>();
        public Decimal Total { get; set; } = 0;
        public bool OrderSuccess { get; set; }

    }
}
