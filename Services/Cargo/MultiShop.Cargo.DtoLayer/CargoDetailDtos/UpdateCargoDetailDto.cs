using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DtoLayer.CargoDetailDtos
{
    public class UpdateCargoDetailDto
{
    public int CargoDetailId { get; set; }
    public int OrderDetailId { get; set; }
    public string VendorId { get; set; }
    public string TrackingNumber { get; set; }
    public int CargoCompanyId { get; set; }
    public string CargoStatus { get; set; }
    public DateTime CreatedDate { get; set; }
}
}
