using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Photos
{
    public class PhotoData
    {
        public int ProductId { get; set; }
        public IFormFile Photo { get; set; }
    }
}
