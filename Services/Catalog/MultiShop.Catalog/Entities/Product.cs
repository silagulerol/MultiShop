using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; }
        
        public string Description { get; set; }

        public string CategoryId { get; set; }

        [BsonIgnore]
        //bir property’nin veritabanına kaydedilmemesini sağlayan bir attribute’tur.
        //Bu property C# tarafında var ama MongoDB’ye yazma.
        public Category Category { get; set; }
    }
}
