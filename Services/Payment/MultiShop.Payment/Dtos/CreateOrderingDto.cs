namespace MultiShop.Payment.Dtos
{
    // Basket'in içindeki her bir Item için oluşturulan Dto
   public class CreateOrderingDto
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }

}
