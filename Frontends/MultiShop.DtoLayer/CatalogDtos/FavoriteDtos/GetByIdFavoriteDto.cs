namespace MultiShop.DtoLayer.CatalogDtos.FavoriteDtos
{
    public class GetByIdFavoriteDto
    {
        public string FavoriteId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
