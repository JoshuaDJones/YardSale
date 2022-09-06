using Core.YardSale.Contracts;
using Core.YardSale.Photos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("GetProduct")]
        public IActionResult GetProduct(int productId)
        {
            var result = _productRepository.GetProduct(productId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProducts")]
        public IActionResult GetProducts(int categoryId)
        {
            var result = _productRepository.GetProductList(categoryId);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateProduct")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            var result = _productRepository.CreateProduct(product);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            var result = _productRepository.UpdateProduct(product);
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public IActionResult DeleteProduct(int productId)
        {
            var result = _productRepository.DeleteProduct(productId);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateThumbnailPhoto")]
        public IActionResult CreateThumbnailPhoto([FromForm] ProductPhotoData productPhotoData)
        {
            var result = _productRepository.CreateThumbnailPhoto(productPhotoData.ThumbNailPhoto);
            return Ok(result.Result);
        }

        [HttpGet]
        [Route("GetSellersProducts")]
        public IActionResult GetSellersProducts(int userId)
        {
            var result = _productRepository.GetSellerProductList(userId);
            return Ok(result);
        }

        [HttpGet]
        [Route("MarkProductSold")]
        public IActionResult MarkProductSold(int productId)
        {
            var result = _productRepository.MarkProductSold(productId);
            return Ok(result);
        }
    } 
}
