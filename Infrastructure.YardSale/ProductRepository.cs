using Azure.Storage.Blobs;
using Core.YardSale.Contracts;
using Core.YardSale.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.YardSale
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly string _containerString = "";
        private readonly string _connectionString = "";

        private readonly BlobServiceClient _blobServiceClient;
        public ProductRepository(IDatabaseRepository databaseRepository, BlobServiceClient blobServiceClient)
        {
            _databaseRepository = databaseRepository;
            _blobServiceClient = blobServiceClient;
        }

        public int CreateProduct(Product product)
        {
            int returnVal = _databaseRepository.GetRetVal("usp_Product_Add", new List<Object> { product.UserId, product.Category.category_id, product.ProductTitle, product.ProductDescription, product.ProductCost, product.ProductThumbnailPhotoUrl ?? "", product.ProductThumbnailPhotoFilename ?? "", product.ProductIsActive, product.ProductIsSold});
            return returnVal;
        }

        public async Task<ProductThumbnailData> CreateThumbnailPhoto(IFormFile photo)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerString);
            var ext = Path.GetExtension(photo.FileName);
            var newFileName = $"{Guid.NewGuid()}{ext}";
            //var blobClient = containerClient.GetBlobClient(newFileName);
            var blobClient = new BlobClient(_connectionString, _containerString, newFileName);

            using var stream = photo.OpenReadStream();
            var result = await blobClient.UploadAsync(stream, true);
            string photoUrl = blobClient.Uri.ToString();

            ProductThumbnailData productThumbnailData = new()
            {
                ThumbnailPhotoUrl = photoUrl,
                ThumbnailPhotoFilename = newFileName
            };

            return productThumbnailData;
        }

        public int DeleteProduct(int productId)
        {
            int returnVal = _databaseRepository.GetRetVal("usp_Product_Del", new List<object> { productId });
            return returnVal;
        }

        public Product GetProduct(int productId)
        {
            DataTable dt = _databaseRepository.GetDT("usp_Product_Get", new List<object> { productId });

            Product product = new();

            product.ProductId = Convert.ToInt32(dt.Rows[0]["product_id"]);
            product.UserId = Convert.ToInt32(dt.Rows[0]["user_id"]);
            product.ProductTitle = dt.Rows[0]["product_title"].ToString() ?? ""; 
            product.ProductDescription = dt.Rows[0]["product_description"].ToString() ?? "";
            product.ProductCost = Convert.ToDecimal(dt.Rows[0]["product_cost"]);
            product.ProductThumbnailPhotoUrl = dt.Rows[0]["product_thumbnail_photo_url"].ToString() ?? "";
            product.ProductThumbnailPhotoFilename = dt.Rows[0]["product_thumbnail_photo_filename"].ToString() ?? "";
            product.ProductIsActive = Convert.ToBoolean(dt.Rows[0]["product_is_active"]);
            product.ProductIsSold = Convert.ToBoolean(dt.Rows[0]["product_is_sold"]);

            Category category = new Category();
            category.category_id = Convert.ToInt32(dt.Rows[0]["category_id"]);

            product.Category = category;

            return product;
        }

        public List<Product> GetProductList(int categoryId)
        {
            List<Product> products = new List<Product>();

            DataTable dt = _databaseRepository.GetDT("usp_Product_Get_List", new List<object> { categoryId } );

            foreach(DataRow row in dt.Rows)
            {
                Category category = new();
                category.category_id = Convert.ToInt32(row["category_id"]);
                Product product = new();
                product.ProductId = Convert.ToInt32(row["product_id"]);
                product.UserId = Convert.ToInt32(row["user_id"]);
                product.Category = category;
                product.ProductTitle = row["product_title"].ToString() ?? "";
                product.ProductDescription = row["product_description"].ToString() ?? "";
                product.ProductCost = Convert.ToDecimal(row["product_cost"]);
                product.ProductThumbnailPhotoUrl = row["product_thumbnail_photo_url"].ToString() ?? "";
                product.ProductThumbnailPhotoFilename = row["product_thumbnail_photo_filename"].ToString() ?? "";
                product.ProductIsActive = Convert.ToBoolean(dt.Rows[0]["product_is_active"]);
                product.ProductIsSold = Convert.ToBoolean(dt.Rows[0]["product_is_sold"]);

                products.Add(product);
            }

            return products;
        }

        public List<Product> GetSellerProductList(int userId)
        {
            List<Product> products = new List<Product>();

            DataTable dt = _databaseRepository.GetDT("usp_Product_Get_Seller_List", new List<object> { userId });

            foreach (DataRow row in dt.Rows)
            {
                Category category = new();
                category.category_id = Convert.ToInt32(row["category_id"]);

                Product product = new();
                product.Category = category;
                product.ProductId = Convert.ToInt32(row["product_id"]);
                product.UserId = Convert.ToInt32(row["user_id"]);
                product.ProductTitle = row["product_title"].ToString() ?? "";
                product.ProductDescription = row["product_description"].ToString() ?? "";
                product.ProductCost = Convert.ToDecimal(row["product_cost"]);
                product.ProductThumbnailPhotoUrl = row["product_thumbnail_photo_url"].ToString() ?? "";
                product.ProductThumbnailPhotoFilename = row["product_thumbnail_photo_filename"].ToString() ?? "";
                product.ProductIsActive = Convert.ToBoolean(row["product_is_active"]);
                product.ProductIsSold = Convert.ToBoolean(row["product_is_sold"]);

                products.Add(product);
            }

            return products;
        }

        public int MarkProductSold(int productId)
        {
            int returnVal = _databaseRepository.GetRetVal("usp_Product_Mark_Sold", new List<object> { productId });
            return returnVal;
        }

        public int UpdateProduct(Product product)
        {
            int returnVal = _databaseRepository.GetRetVal("usp_Product_Upd", new List<object>{ product.ProductId, product.UserId, product.Category.category_id, product.ProductTitle, product.ProductDescription, product.ProductCost, product.ProductIsActive, product.ProductIsSold });
            return returnVal;
        }
    }
}