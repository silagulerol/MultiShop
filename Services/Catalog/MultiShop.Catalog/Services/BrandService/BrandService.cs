using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.BrandDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.BrandService
{
    public class BrandService : IBrandService
    {
        private readonly IMongoCollection<Brand> _brandCollection;
        private readonly IMapper _mapper;

        public BrandService(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            _mapper = mapper;
            var MongoClient = new MongoClient(_databaseSettings.ConnectionString);
            var database = MongoClient.GetDatabase(_databaseSettings.DatabaseName);
            _brandCollection = database.GetCollection<Brand>(_databaseSettings.BrandCollectionName);
        }

        public async Task CreateBrandAsync(CreateBrandDto createBrandDto)
        {
            await _brandCollection.InsertOneAsync(_mapper.Map<Brand>(createBrandDto));
        }

        public async Task DeleteBrandAsync(string id)
        {
            await _brandCollection.FindOneAndDeleteAsync(x => x.BrandId == id);
        }

        public async Task<List<ResultBrandDto>> GetAllBrandAsync()
        {
            var values = await _brandCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultBrandDto>>(values);
        }

        public async Task<ResultBrandDto> GetByIdBrandAsync(string id)
        {
            var value = await _brandCollection.Find(x => x.BrandId == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultBrandDto>(value);
        }

        public async Task UpdateBrandAsync(UpdateBrandDto updateBrandDto)
        {
            var value = _mapper.Map<Brand>(updateBrandDto);
            await _brandCollection.FindOneAndReplaceAsync(x => x.BrandId == value.BrandId, value);
        }
    }
}
