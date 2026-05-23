using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.DtoLayer.IdentityDtos.RegisterDtos
{
    public class CreateRegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } = "Customer";

        public string? StoreName { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}