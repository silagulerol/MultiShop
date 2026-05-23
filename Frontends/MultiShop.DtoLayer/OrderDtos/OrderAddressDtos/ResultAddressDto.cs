namespace MultiShop.DtoLayer.OrderDtos.OrderAddressDtos
{
    public class ResultAddressDto
    {
        public int AddressId { get; set; }
        public string UserId { get; set; } = "";
        public string City { get; set; } = "";
        public string District { get; set; } = "";
        public string Detail { get; set; } = "";
    }
}
