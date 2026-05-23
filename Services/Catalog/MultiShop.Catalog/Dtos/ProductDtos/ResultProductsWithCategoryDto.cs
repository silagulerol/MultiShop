using MultiShop.Catalog.Dtos.BrandDtos;
using MultiShop.Catalog.Dtos.CategoryDtos;

namespace MultiShop.Catalog.Dtos.ProductDtos
{
    public class ResultProductsWithCategoryDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Slug { get; set; }
        public string BrandId { get; set; }
        public string? BrandName { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public ResultCategoryDto Category { get; set; }
        public ResultBrandDto? Brand { get; set; }
        public string VendorId { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? StartingDiscountedPrice { get; set; }
        public int? MaxDiscountRate { get; set; }
        public string? MainImageUrl { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int FavoriteCount { get; set; }
        public int QuestionCount { get; set; }
        public int ViewCount { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsWomenEntrepreneurProduct { get; set; }
        public bool IsRecyclableProduct { get; set; }
        public string? BadgeText { get; set; }
        public bool IsFreeShipping { get; set; }
        public string? ShippingInfo { get; set; }
        public string? ReturnPolicy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
