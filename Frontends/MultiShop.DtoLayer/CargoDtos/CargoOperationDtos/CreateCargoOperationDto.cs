namespace MultiShop.DtoLayer.CargoDtos.CargoOperationDtos
{
    public class CreateCargoOperationDto
    {
        public int CargoDetailId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime OperationDate { get; set; }
    }
}