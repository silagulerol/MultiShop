namespace MultiShop.DtoLayer.CargoDtos.CargoDetailDtos
{
    public class CreateCargoDetailDto
    {
        public int OrderDetailId { get; set; }
        public string VendorId { get; set; }
        public int CargoCompanyId { get; set; }
        public string CargoStatus { get; set; }
    }
}