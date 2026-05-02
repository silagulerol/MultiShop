using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiShop.IdentityServer.Data;
using MultiShop.IdentityServer.Models;
using Serilog;

namespace MultiShop.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();

            services.AddLogging();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedRole(roleMgr, "Admin");
            SeedRole(roleMgr, "Customer");
            SeedRole(roleMgr, "Vendor");

            SeedUser(
                userMgr,
                username: "admin01",
                email: "admin01@multishop.com",
                password: "Admin123*",
                role: "Admin",
                name: "Admin User",
                givenName: "Admin",
                familyName: "User"
            );

            SeedUser(
                userMgr,
                username: "sam01",
                email: "sam01@multishop.com",
                password: "1111aA*",
                role: "Customer",
                name: "Sam Customer",
                givenName: "Sam",
                familyName: "Customer"
            );

            SeedUser(
                userMgr,
                username: "vendor01",
                email: "vendor01@multishop.com",
                password: "Vendor123*",
                role: "Vendor",
                name: "Vendor User",
                givenName: "Vendor",
                familyName: "User"
            );
        }

        private static void SeedRole(RoleManager<IdentityRole> roleMgr, string roleName)
        {
            var roleExists = roleMgr.RoleExistsAsync(roleName).Result;

            if (!roleExists)
            {
                var result = roleMgr.CreateAsync(new IdentityRole(roleName)).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug($"{roleName} role created");
            }
            else
            {
                Log.Debug($"{roleName} role already exists");
            }
        }

        private static void SeedUser(
            UserManager<ApplicationUser> userMgr,
            string username,
            string email,
            string password,
            string role,
            string name,
            string givenName,
            string familyName)
        {
            var user = userMgr.FindByNameAsync(username).Result;

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true
                };

                var createResult = userMgr.CreateAsync(user, password).Result;

                if (!createResult.Succeeded)
                {
                    throw new Exception(createResult.Errors.First().Description);
                }

                var roleResult = userMgr.AddToRoleAsync(user, role).Result;

                if (!roleResult.Succeeded)
                {
                    throw new Exception(roleResult.Errors.First().Description);
                }

                var claimResult = userMgr.AddClaimsAsync(user, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, name),
                    new Claim(JwtClaimTypes.GivenName, givenName),
                    new Claim(JwtClaimTypes.FamilyName, familyName),
                    new Claim(JwtClaimTypes.Email, email),
                    new Claim(JwtClaimTypes.Role, role)
                }).Result;

                if (!claimResult.Succeeded)
                {
                    throw new Exception(claimResult.Errors.First().Description);
                }

                Log.Debug($"{username} created with {role} role");
            }
            else
            {
                Log.Debug($"{username} already exists");
            }
        }
    }
}