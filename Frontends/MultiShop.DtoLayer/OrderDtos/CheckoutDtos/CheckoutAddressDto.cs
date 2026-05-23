using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.DtoLayer.OrderDtos.CheckoutDtos
{
    public class CheckoutAddressDto
    {
        public int? SelectedAddressId { get; set; }
        public bool UseNewAddress { get; set; }
        public CreateAddressDto NewAddress { get; set; } = new();
        public List<ResultAddressDto> ExistingAddresses { get; set; } = new();
    }
}
