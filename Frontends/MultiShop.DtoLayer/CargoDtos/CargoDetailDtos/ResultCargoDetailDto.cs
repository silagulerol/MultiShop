namespace MultiShop.DtoLayer.CargoDtos.CargoDetailDtos
{
    public class ResultCargoDetailDto
    {
        public int CargoDetailId { get; set; }
        public int OrderDetailId { get; set; }
        public string TrackingNumber { get; set; }
        public string CargoStatus { get; set; }
        public int CargoCompanyId { get; set; }
        public string? CargoCompanyName { get; set; }
        
    }
}