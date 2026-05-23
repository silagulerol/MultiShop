using System;

namespace MultiShop.Cargo.EntityLayer.Concrete
{
    // kargo hareket geçmişi
    public class CargoOperation
    {
        public int CargoOperationId { get; set; }

        public int CargoDetailId { get; set; }
        public CargoDetail? CargoDetail { get; set; }

        public string Status { get; set; } = "Preparing";
        public string Description { get; set; } = null!;

        public DateTime OperationDate { get; set; }
    }
}