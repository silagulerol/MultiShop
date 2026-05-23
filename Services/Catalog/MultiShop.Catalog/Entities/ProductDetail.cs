using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    public class ProductDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductDetailId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ProductVariantId { get; set; }

        public string ProductLongDescription { get; set; } = null!;
        public string ProductInformation { get; set; } = null!;
        public List<string> Highlights { get; set; } = new List<string>();
        public Dictionary<string, string> Specifications { get; set; } = new Dictionary<string, string>();
        public string? Material { get; set; }
        public string? CareInstructions { get; set; }
        public string? PackageContent { get; set; }
        public string? WarrantyInfo { get; set; }
        public string? SafetyInfo { get; set; }
        public bool IsCustomizable { get; set; }
        public string? PersonalizationInstructions { get; set; }
        public int? PersonalizationMaxLength { get; set; }
        public string DetailScope { get; set; } = "ProductLevel";
        public DateTime CreatedDate { get; set; }

        [BsonIgnore]
        public Product Product { get; set; } = null!;

        [BsonIgnore]
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
