namespace YardSale.Models
{
    public class PurchaseInfo
    {
        public string productKeys { get; set; } = "";
        public decimal customerTotal { get; set; }
        public string customerAddress { get; set; } = "";
    }
}