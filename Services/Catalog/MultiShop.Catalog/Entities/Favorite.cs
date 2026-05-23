using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    public class Favorite
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FavoriteId { get; set; } = null!;
        public string UserId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        [BsonIgnore]
        public Product Product { get; set; } = null!;
    }
}
