using System;

namespace MultiShop.Cargo.DtoLayer.CargoOperationDetailDtos
{
    public class UpdateCargoOperationDto
    {
        public int CargoOperationId { get; set; }
        public int CargoDetailId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
