using IdentityServer4.Hosting.LocalApiAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.IdentityServer.Dtos;
using MultiShop.IdentityServer.Models;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace MultiShop.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RegistersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var values =await _userManager.Users.ToListAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(UserRegisterDto userRegisterDto)
        {
            var values = new ApplicationUser()
            {
                Name = userRegisterDto.Name,
                Email = userRegisterDto.Email,
                PasswordHash = userRegisterDto.Password,
                Surname = userRegisterDto.Surname,
                UserName = userRegisterDto.UserName,
            };

           var result =  await _userManager.CreateAsync(values, userRegisterDto.Password);
            if (result.Succeeded)
            {
                return Ok("Usser is regitered successfully");
            }else
            {
                return BadRequest(result.Errors);
                //return Ok("There is an error, please try again");
            }
        }
    }
}
