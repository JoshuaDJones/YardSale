using Core.YardSale.Contracts;
using Core.YardSale.Products;

namespace YardSale.ViewModels
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
