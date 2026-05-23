
namespace MultiShop.Cargo.DtoLayer.CargoDetailDtos
{
    public class ResultCargoDetailDto
    {
        public int CargoDetailId { get; set; }
        public int OrderDetailId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public string CargoStatus { get; set; } = null!;
        public int CargoCompanyId { get; set; }
        public string? CargoCompanyName { get; set; }
    }
}
