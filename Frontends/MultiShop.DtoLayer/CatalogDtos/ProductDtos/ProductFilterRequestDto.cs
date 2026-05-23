namespace MultiShop.DtoLayer.CatalogDtos.ProductDtos
{
    public class ProductFilterRequestDto
    {
        public string? CategoryId { get; set; }
        public List<string> Brands { get; set; } = new();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string> Colors { get; set; } = new();
        public List<string> Sizes { get; set; } = new();
        public List<string> Materials { get; set; } = new();
        public List<string> Capacities { get; set; } = new();
        public List<string> Styles { get; set; } = new();
        public double? Rating { get; set; }
        public string? SortBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 24;
        public bool? IsFreeShipping { get; set; }
        public bool? IsBestSeller { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsRecyclable { get; set; }
        public bool? IsWomenEntrepreneur { get; set; }
        public bool? InStock { get; set; }
    }
}
