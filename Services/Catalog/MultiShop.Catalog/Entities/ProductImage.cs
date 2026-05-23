using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    public class ProductImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductImageId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ProductVariantId { get; set; }

        public string ImageUrl { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public bool IsMainImage { get; set; }
        public string? ImageAltText { get; set; }
        public string? ImageType { get; set; }
        public DateTime CreatedDate { get; set; }

        [BsonIgnore]
        public Product Product { get; set; } = null!;

        [BsonIgnore]
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
