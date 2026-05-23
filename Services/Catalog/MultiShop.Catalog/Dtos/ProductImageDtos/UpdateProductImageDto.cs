namespace MultiShop.Catalog.Dtos.ProductImageDtos
{
    public class UpdateProductImageDto
    {
        public string ProductImageId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public string? ProductVariantId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public bool IsMainImage { get; set; }
        public string? ImageAltText { get; set; }
        public string ImageType { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
