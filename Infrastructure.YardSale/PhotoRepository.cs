using Azure.Storage.Blobs;
using Core.YardSale.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.YardSale
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerString = "";
        private readonly IDatabaseRepository _databaseRepository;
        public PhotoRepository(BlobServiceClient blobSerivceClient, IDatabaseRepository databaseRepository)
        {
            _blobServiceClient = blobSerivceClient;
            _databaseRepository = databaseRepository;
        }
        public async Task<int> CreatePhoto(IFormFile photo, int productId)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerString);
            var ext = Path.GetExtension(photo.FileName);
            var newFileName = $"{Guid.NewGuid()}{ext}";
            var blobClient = containerClient.GetBlobClient(newFileName);

            using var stream = photo.OpenReadStream();
            var result = await blobClient.UploadAsync(stream, true);
            string photoUrl = blobClient.Uri.ToString();

            int retVal = _databaseRepository.GetRetVal("usp_photo_add", new List<object>(){ productId, photoUrl, newFileName });
            return retVal;
        }

        public int DeletePhoto(int photoId)
        {
            int retVal = _databaseRepository.GetRetVal("usp_Photo_Del", new List<object>() { photoId });
            return retVal;
        }

        public List<Photo> GetProductPhotos(int productId)
        {
            List<Photo> photos = new();
            DataTable dt = _databaseRepository.GetDT("usp_Photo_Get_List", new List<object> { productId });

            foreach(DataRow row in dt.Rows)
            {
                Photo photo = new()
                {
                    PhotoId = Convert.ToInt32(row["photo_id"]),
                    ProductId = Convert.ToInt32(row["product_id"]),
                    PhotoUrl = row["photo_url"].ToString(),
                    PhotoFileName = row["photo_file_name"].ToString()
                };

                photos.Add(photo);  
            }
            return photos;
        }

    }
}
