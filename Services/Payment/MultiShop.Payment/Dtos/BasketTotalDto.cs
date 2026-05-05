namespace MultiShop.Payment.Dtos
{
    // Basket'in içindeki her bir Item için oluşturulan Dto
   public class BasketTotalDto
    {
        public string UserId { get; set; }
        public List<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto>();
        public decimal TotalPrice { get; set; }
    }

}
