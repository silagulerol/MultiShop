namespace MultiShop.IdentityServer.Dtos
{
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } 
        public string? StoreName { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
