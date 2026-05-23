using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    public class ProductVariant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductVariantId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
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

        [BsonIgnore]
        public Product Product { get; set; } = null!;
    }
}
