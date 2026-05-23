namespace MultiShop.DtoLayer.CatalogDtos.ProductDtos
{
    public class ProductFilterResponseDto
    {
        public List<ResultProductWithCategoryDto> Products { get; set; } = new();
        public List<ProductFilterOptionDto> Brands { get; set; } = new();
        public List<ProductFilterOptionDto> Colors { get; set; } = new();
        public List<ProductFilterOptionDto> Sizes { get; set; } = new();
        public List<ProductFilterOptionDto> Materials { get; set; } = new();
        public List<ProductFilterOptionDto> Capacities { get; set; } = new();
        public List<ProductFilterOptionDto> Styles { get; set; } = new();
        public string? CategoryName { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
