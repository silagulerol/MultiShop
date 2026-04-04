using Microsoft.EntityFrameworkCore;
using MultiShop.Comment.Entities;

namespace MultiShop.Comment.Context
{
    public class CommentContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //connection adresini buraya yazıyoruz
            optionsBuilder.UseSqlServer("Server=localhost,1442;Database=MultiShopCommentDb;User=sa;Password=Sero1234.;TrustServerCertificate=True;");
        }

        public DbSet<UserComment> UserComments { get; set; }
    }
}
