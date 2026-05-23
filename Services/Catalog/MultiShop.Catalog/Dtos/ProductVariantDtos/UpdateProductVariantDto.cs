namespace MultiShop.Catalog.Dtos.ProductVariantDtos
{
    public class UpdateProductVariantDto
    {
        public string ProductVariantId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public string VariantName { get; set; } = null!;
        public string? Sku { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ColorHexCode { get; set; }
        public string? MaterialOption { get; set; }
        public string? CapacityOption { get; set; }
        public string? StyleOption { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int DiscountRate { get; set; }
        public int Stock { get; set; }
        public int MaxOrderQuantity { get; set; }
        public string? MainImageUrl { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
