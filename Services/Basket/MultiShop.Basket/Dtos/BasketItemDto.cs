namespace MultiShop.Basket.Dtos
{
    // Basket'in içindeki her bir Item için oluşturulan Dto
    public class BasketItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int BasketId { get; set; }
    }
}
