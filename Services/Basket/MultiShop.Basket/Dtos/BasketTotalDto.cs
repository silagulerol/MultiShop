namespace MultiShop.Basket.Dtos
{
    public class BasketTotalDto
    {
        // Basket hangi kullanıcıya aitse
        public string UserId { get; set; } 
        public string DiscountCode { get; set; }
        public int DiscountRate { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public decimal TotalPrice { get => BasketItems.Sum(x => x.UnitPrice * x.Quantity); }

    }
}
