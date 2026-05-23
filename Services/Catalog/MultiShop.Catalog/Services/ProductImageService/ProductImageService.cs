using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductImageService
{
    public class ProductImageService : IProductImageService
    {
        private readonly IMongoCollection<ProductImage> _productImageCollection;
        private readonly IMapper _mapper;

        public ProductImageService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _productImageCollection = database.GetCollection<ProductImage>(databaseSettings.ProductImageCollectionName);
            _mapper = mapper;
        }

        public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
        {
            await _productImageCollection.InsertOneAsync(_mapper.Map<ProductImage>(createProductImageDto));
        }

        public async Task DeleteProductImageAsync(string id)
        {
            await _productImageCollection.DeleteOneAsync(x => x.ProductImageId == id);
        }

        public async Task<List<ResultProductImageDto>> GetAllProductImageAsync()
        {
            var values = await _productImageCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultProductImageDto>>(values);
        }

        public async Task<GetByIdProductImageDto> GetByIdProductImageAsync(string id)
        {
            var value = await _productImageCollection.Find(x => x.ProductImageId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductImageDto>(value);
        }


        public async Task<List<ResultProductImageDto>> GetImagesByProductIdAsync(string productId)
        {
            var values = await _productImageCollection
                .Find(x => x.ProductId == productId && x.ProductVariantId == null)
                .SortBy(x => x.DisplayOrder)
                .ToListAsync();

            return _mapper.Map<List<ResultProductImageDto>>(values);
        }

        public async Task<List<ResultProductImageDto>> GetImagesByVariantIdAsync(string productVariantId)
        {
            var values = await _productImageCollection
                .Find(x => x.ProductVariantId == productVariantId)
                .SortBy(x => x.DisplayOrder)
                .ToListAsync();

            return _mapper.Map<List<ResultProductImageDto>>(values);
        }

        public async Task<GetByIdProductImageDto> GetMainImageByProductIdAsync(string productId)
        {
            var value = await _productImageCollection
                .Find(x => x.ProductId == productId && x.ProductVariantId == null && x.IsMainImage)
                .SortBy(x => x.DisplayOrder)
                .FirstOrDefaultAsync();

            return _mapper.Map<GetByIdProductImageDto>(value);
        }

        public async Task<GetByIdProductImageDto> GetMainImageByVariantIdAsync(string productVariantId)
        {
            var value = await _productImageCollection
                .Find(x => x.ProductVariantId == productVariantId && x.IsMainImage)
                .SortBy(x => x.DisplayOrder)
                .FirstOrDefaultAsync();

            return _mapper.Map<GetByIdProductImageDto>(value);
        }

        public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
        {
            var value = _mapper.Map<ProductImage>(updateProductImageDto);
            await _productImageCollection.ReplaceOneAsync(x => x.ProductImageId == updateProductImageDto.ProductImageId, value);
        }
    }
}
