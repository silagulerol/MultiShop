using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.IdentityServer.Data;
using MultiShop.IdentityServer.Dtos;
using MultiShop.IdentityServer.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MultiShop.IdentityServer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RegistersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var values = await _userManager.Users.ToListAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(UserRegisterDto userRegisterDto)
        {
            var role = userRegisterDto.Role == "Vendor" ? "Vendor" : "Customer";

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = new ApplicationUser
            {
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
                Email = userRegisterDto.Email,
                UserName = userRegisterDto.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, role);

            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim(JwtClaimTypes.GivenName, user.Name ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.Surname ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? ""),
                new Claim(JwtClaimTypes.Role, role)
            });

            if (role == "Vendor")
            {
                var vendorProfile = new VendorProfile
                {
                    UserId = user.Id,
                    StoreName = userRegisterDto.StoreName,
                    CompanyName = userRegisterDto.CompanyName,
                    TaxNumber = userRegisterDto.TaxNumber,
                    Phone = userRegisterDto.Phone,
                    Address = userRegisterDto.Address,
                    CreatedDate = DateTime.Now,
                    IsApproved = true
                };

                _context.VendorProfiles.Add(vendorProfile);
                await _context.SaveChangesAsync();
            }

            return Ok("User is registered successfully");
        }
    }
}