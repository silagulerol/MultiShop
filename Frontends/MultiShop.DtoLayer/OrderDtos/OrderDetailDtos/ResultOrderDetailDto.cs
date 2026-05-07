namespace MultiShop.DtoLayer.OrderDtos.OrderDetailDtos
{
    public class ResultOrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public string ProductId { get; set; }
        public string? VendorId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotalPrice { get; set; }
        public int OrderingId { get; set; }
        public string OrderStatus { get; set; } 
        public bool HasShipment { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShipmentStatus { get; set; }
        public string? VendorName { get; set; }
    }
}