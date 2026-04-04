namespace MultiShop.DtoLayer.CatalogDtos.ProductImageDtos
{
    public class GetByIdProductImageDto
    {
        public string ProductImageId { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }

        // Product 1 --- N ProductImages
        public string ProductId { get; set; }
    }
}
