using Core.YardSale.Contracts;

namespace YardSale.ViewModels
{
    public class SellerProductsViewModal
    {
        public ProductCreateViewModel productCreateViewModel { get; set; }
        public Product productEdit { get; set; } = new();
        public List<Product> products { get; set; }
        public bool IsEdit { get; set; } = false;
    }
}
