using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Products
{
    public class Category
    {
        [Required]
        [Range(1,6, ErrorMessage = "You must select a category.")]
        public int category_id { get; set; }
        public string category_desc { get; set; } = string.Empty;
    }

    public enum CategoryType
    {
        Tools = 1,
        [Display(Name = "Kitchen Appliances")]
        KitchenAppliances = 2,
        [Display(Name = "Yard Supplies")]
        YardSupplies = 3,
        [Display(Name = "Home Decoration")]
        HomeDecor = 4,
        Automotive = 5,
        Clothing = 6
    }
}
