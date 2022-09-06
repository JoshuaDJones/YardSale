using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Contracts
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public int ProductId { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public string PhotoFileName { get; set; } = string.Empty;
    }
}
