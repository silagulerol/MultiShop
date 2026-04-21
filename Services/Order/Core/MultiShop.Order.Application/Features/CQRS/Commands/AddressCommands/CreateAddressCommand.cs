using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Commands.AddressCommands
{
    public class CreateAddressCommand
    {
        /* Parametre olarak userId, city, district, detail alacağım ama addressId almıyorum (otomatik artan)
           
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateAddressCommand command)
        
        ASP.NET Core, JSON body’yi okuyup property’leri set eder.
        Bunun için parametresiz constructor(default) veya set edilebilir property’ler gerekir.
        */
        public int AddressId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Description { get; set; }
        public string ZipCode { get; set; }
    }
}
