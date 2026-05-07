using System;
using Microsoft.AspNetCore.Identity;

namespace MultiShop.IdentityServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class VendorProfile
    {
        public int VendorProfileId { get; set; }

        public string UserId { get; set; } = null!; // ApplicationUser.Id
        public ApplicationUser User { get; set; } = null!;

        public string StoreName { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string TaxNumber { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
