using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductVariantDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductVariantService
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IMongoCollection<ProductVariant> _productVariantCollection;
        private readonly IMapper _mapper;

        public ProductVariantService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _productVariantCollection = database.GetCollection<ProductVariant>(databaseSettings.ProductVariantCollectionName);
            _mapper = mapper;
        }

        public async Task CreateProductVariantAsync(CreateProductVariantDto createProductVariantDto)
        {
            await _productVariantCollection.InsertOneAsync(_mapper.Map<ProductVariant>(createProductVariantDto));
        }

        public async Task DeleteProductVariantAsync(string id)
        {
            await _productVariantCollection.DeleteOneAsync(x => x.ProductVariantId == id);
        }

        public async Task<List<ResultProductVariantDto>> GetAllProductVariantAsync()
        {
            var values = await _productVariantCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultProductVariantDto>>(values);
        }

        public async Task<GetByIdProductVariantDto> GetByIdProductVariantAsync(string id)
        {
            var value = await _productVariantCollection.Find(x => x.ProductVariantId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductVariantDto>(value);
        }

        public async Task<List<ResultProductVariantDto>> GetProductVariantsByProductIdAsync(string productId)
        {
            var values = await _productVariantCollection
                .Find(BuildProductVariantFilter(productId))
                .SortByDescending(x => x.IsDefault)
                .ThenByDescending(x => x.IsAvailable)
                .ThenBy(x => x.VariantName)
                .ToListAsync();

            return _mapper.Map<List<ResultProductVariantDto>>(values);
        }

        public async Task<List<ResultProductVariantDto>> GetAvailableProductVariantsByProductIdAsync(string productId)
        {
            var values = await _productVariantCollection
                .Find(BuildProductVariantFilter(productId, true))
                .SortByDescending(x => x.IsDefault)
                .ThenBy(x => x.VariantName)
                .ToListAsync();

            return _mapper.Map<List<ResultProductVariantDto>>(values);
        }

        public async Task UpdateProductVariantAsync(UpdateProductVariantDto updateProductVariantDto)
        {
            var value = _mapper.Map<ProductVariant>(updateProductVariantDto);
            await _productVariantCollection.ReplaceOneAsync(x => x.ProductVariantId == value.ProductVariantId, value);
        }

        private static FilterDefinition<ProductVariant> BuildProductVariantFilter(string productId, bool? isAvailable = null)
        {
            var filterBuilder = Builders<ProductVariant>.Filter;
            var filter = filterBuilder.Eq(x => x.ProductId, productId);

            if (isAvailable.HasValue)
            {
                filter &= filterBuilder.Eq(x => x.IsAvailable, isAvailable.Value);
            }

            return filter;
        }
    }
}
