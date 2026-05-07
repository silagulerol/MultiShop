using System;

namespace MultiShop.Cargo.EntityLayer.Concrete
{
    public class CargoDetail
    {
        public int CargoDetailId { get; set; }

        // hangi sipariş ürünü kargoya verildi
        public int OrderDetailId { get; set; }

        // hangi satıcı kargoladı
        public string VendorId { get; set; } = null!;

        // takip numarası
        public string TrackingNumber { get; set; } = null!;

        // hangi kargo firması
        public int CargoCompanyId { get; set; }
        public CargoCompany? CargoCompany { get; set; }

        public DateTime CreatedDate { get; set; }

        // kargonun mevcut durumu
        public string CargoStatus { get; set; } = "Preparing";
    }
}