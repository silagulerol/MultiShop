namespace MultiShop.Catalog.Dtos.FavoriteDtos
{
    public class ResultFavoriteDto
    {
        public string FavoriteId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string? ProductName { get; set; }
        public string? MainImageUrl { get; set; }
        public decimal? StartingPrice { get; set; }
    }
}
