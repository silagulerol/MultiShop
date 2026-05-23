namespace MultiShop.DtoLayer.CatalogDtos.FavoriteDtos
{
    public class UpdateFavoriteDto
    {
        public string FavoriteId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
