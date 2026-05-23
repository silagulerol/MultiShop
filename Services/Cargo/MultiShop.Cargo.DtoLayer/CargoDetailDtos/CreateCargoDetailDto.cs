using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DtoLayer.CargoDetailDtos
{
    public class CreateCargoDetailDto
{
    public int OrderDetailId { get; set; }
    public string VendorId { get; set; }
    public int CargoCompanyId { get; set; }
    public string CargoStatus { get; set; }
}
}
