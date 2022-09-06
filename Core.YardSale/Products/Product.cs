using Core.YardSale.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Contracts
{
    public class Product
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public Category Category { get; set; }
        [Required(ErrorMessage = "You must enter a product title.")]
        public string ProductTitle { get; set; } = string.Empty;
        [Required(ErrorMessage = "You must enter a product description.")]
        public string ProductDescription { get; set; } = string.Empty;
        [Required(ErrorMessage = "You must enter a product price.")]

        public Decimal ProductCost { get; set; }
        public string? ProductThumbnailPhotoUrl { get; set; } = "";
        public string? ProductThumbnailPhotoFilename { get; set; } = "";
        public bool ProductIsActive { get; set; }
        public bool ProductIsSold { get; set; }
    }
}
