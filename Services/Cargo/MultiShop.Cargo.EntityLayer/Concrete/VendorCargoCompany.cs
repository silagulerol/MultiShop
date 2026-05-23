namespace MultiShop.Cargo.EntityLayer.Concrete
{
    public class VendorCargoCompany
    {
        public int VendorCargoCompanyId { get; set; }
        public string VendorId { get; set; }
        public int CargoCompanyId { get; set; }
        public CargoCompany CargoCompany { get; set; }
    }
}