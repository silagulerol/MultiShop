namespace MultiShop.DtoLayer.PaymentDtos
{
    public class PaymentResultDto
    {
        public string Message { get; set; }
        public int OrderingId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}