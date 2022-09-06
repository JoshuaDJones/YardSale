using Core.YardSale.Contracts;

namespace YardSale.ViewModels
{
    public class BrowseViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int CategoryId { get; set; }
    }
}
