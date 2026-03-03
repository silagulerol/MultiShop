using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MultiShop.Discount.Entities;
using System.Data;

namespace MultiShop.Discount.Context
{
    public class DapperContext : DbContext
    {
        //Dapper:Sadece SQL çalıştırır. Ama bağlantıyı sen açarsın.
        //Production’da: Farklı environment var. Docker var ,Azure var ,Secrets var
        //Bu yüzden connectionString config’ten okunur.

        /*Dapper neden IConfiguration kullanır?
         -Connection string environment’a göre değişir
         - Hardcode edilmez
         -appsettings’ten okunur
         -Güvenli deployment sağlar
         */
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MultiShopDiscountDb;integrated Security=True;TrustServerCertificate=True;");
        }

        public DbSet<Coupon> Coupons { get; set; }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    }

}
