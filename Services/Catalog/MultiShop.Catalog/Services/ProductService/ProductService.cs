using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;
using System;

namespace MultiShop.Catalog.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            await _productCollection.InsertOneAsync(_mapper.Map<Product>(createProductDto));
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productCollection.DeleteOneAsync(x => x.ProductId == id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var values = await _productCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultProductDto>>(values);
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var value = await _productCollection.Find(x => x.ProductId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDto>(value);

        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var value = _mapper.Map<Product>(updateProductDto);
            await _productCollection.ReplaceOneAsync(x => x.ProductId == value.ProductId, value);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            //tüm product'ları db'den çektik
            var values = await _productCollection.Find(_ => true).ToListAsync();
            // ResultProductsWithCategoryDto nesnesindeki Category property'sini
            // CategoryId'si product ile aynı olan Category nesnesi ile doldurduk.
            foreach (var item in values)
            {
                item.Category = await _categoryCollection.Find(x => x.CategoryId == item.CategoryId).FirstAsync();
            }
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }


        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId)
        {
            var values = await _productCollection.Find(x => x.CategoryId == CategoryId).ToListAsync();
            
            foreach (var item in values)
            {
                item.Category = await _categoryCollection.Find(x => x.CategoryId == item.CategoryId).FirstAsync();
            }
            
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }
    }
}
