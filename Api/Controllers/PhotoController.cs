using Core.YardSale.Contracts;
using Core.YardSale.Photos;
using Microsoft.AspNetCore.Mvc;

namespace Api.YardSale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;
        public PhotoController(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        [HttpPost]
        [Route("CreatePhoto")]
        public IActionResult CreatePhoto([FromForm] PhotoData photoData)
        {
            var result = _photoRepository.CreatePhoto(photoData.Photo, photoData.ProductId);
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeletePhoto")]
        public IActionResult DeletePhoto(int photoId)
        {
            var result = _photoRepository.DeletePhoto(photoId);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductPhotos")]
        public IActionResult GetProductPhotos(int productId)
        {
            var result = _photoRepository.GetProductPhotos(productId);
            return Ok(result);
        }
    }
}