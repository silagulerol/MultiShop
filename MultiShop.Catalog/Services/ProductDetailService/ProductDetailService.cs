using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductDetailService
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IMongoCollection<ProductDetail> _productDetailCollection;
        private readonly IMapper _mapper;

        public ProductDetailService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            _mapper = mapper;
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _productDetailCollection = database.GetCollection<ProductDetail>(databaseSettings.ProductDetailCollectionName);
        }
        public async Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto)
        {
            await _productDetailCollection.InsertOneAsync(_mapper.Map<ProductDetail>(createProductDetailDto));
        }

        public async Task DeleteProductDetailAsync(string id)
        {
            await _productDetailCollection.DeleteOneAsync(x => x.ProductDetailId == id);
        }

        public async Task<List<ResultProductDetailDto>> GetAllProductDetailAsync()
        {
            var values = await _productDetailCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultProductDetailDto>>(values);
        }

        public async Task<GetByIdProductDetailDto> GetByIdProductDetailAsync(string id)
        {
            var value = await _productDetailCollection.Find(x => x.ProductDetailId == id)
                .FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDetailDto>(value);
        }

        public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
        {
            var value = _mapper.Map<ProductDetail>(updateProductDetailDto);
            await _productDetailCollection.ReplaceOneAsync(x => x.ProductDetailId == value.ProductDetailId, value);
        }
    }
}
