using Core.YardSale.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Contracts
{
    public interface IProductRepository
    {
        public int CreateProduct(Product product);
        public Task<ProductThumbnailData> CreateThumbnailPhoto(IFormFile photo);
        public int DeleteProduct(int productId);
        public int UpdateProduct(Product product);
        public Product GetProduct(int productId);
        public List<Product> GetProductList(int categoryId);
        public List<Product> GetSellerProductList(int userId);
        public int MarkProductSold(int productId);
    }
}
