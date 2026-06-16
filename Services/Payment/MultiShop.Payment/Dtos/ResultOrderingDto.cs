namespace MultiShop.Payment.Dtos
{
    // Basket'in içindeki her bir Item için oluşturulan Dto
   public class ResultOrderingDto
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }

}
