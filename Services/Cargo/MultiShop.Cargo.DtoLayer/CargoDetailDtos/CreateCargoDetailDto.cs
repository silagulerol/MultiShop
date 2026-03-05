using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DtoLayer.CargoDetailDtos
{
    public class CreateCargoDetailDto
    {

        //Göderici firma hakkında- Sender
        public string SenderCustomer { get; set; }

        //Cargou alan müşteriyi User tablosundan çekicez bu nedenle Id string -  
        public string ReceiverCustomer { get; set; }
        public int Barcode { get; set; }
        public int CargoCompanyId { get; set; }
    }
}
