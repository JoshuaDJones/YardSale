using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Contracts
{
    public interface IPhotoRepository
    {
        Task<int> CreatePhoto(IFormFile photo, int productId);
        int DeletePhoto(int photoId);
        List<Photo> GetProductPhotos(int productId);

    }
}
