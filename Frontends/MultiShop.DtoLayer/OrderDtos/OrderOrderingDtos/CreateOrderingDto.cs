namespace MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos
{
    public class CreateOrderingDto
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string? PaymentMethod { get; set; }
        public int AddressId { get; set; }
    }
}
