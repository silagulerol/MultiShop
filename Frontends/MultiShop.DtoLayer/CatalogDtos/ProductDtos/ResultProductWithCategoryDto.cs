using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;

namespace MultiShop.DtoLayer.CatalogDtos.ProductDtos
{
    public class ResultProductWithCategoryDto
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string BrandId { get; set; } = null!;
        public string? BrandName { get; set; }
        public string Description { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public ResultCategoryDto Category { get; set; } = null!;
        public string VendorId { get; set; } = null!;
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
