namespace MultiShop.Payment.Dtos
{
    // Basket'in içindeki her bir Item için oluşturulan Dto
   public class CreateOrderDetailDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotalPrice { get; set; }
        public int OrderingId { get; set; }
        public string VendorId { get; set; }
    }

}
