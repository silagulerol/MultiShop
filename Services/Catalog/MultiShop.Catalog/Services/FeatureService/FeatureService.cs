using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.FeatureDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FeatureService
{
    public class FeatureService : IFeatureService
    {
        private readonly IMongoCollection<Feature> _featureCollection;
        private readonly IMapper _mapper;

        public FeatureService(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            _mapper = mapper;
            var MongoClient = new MongoClient(_databaseSettings.ConnectionString);
            var database = MongoClient.GetDatabase(_databaseSettings.DatabaseName);
            _featureCollection = database.GetCollection<Feature>(_databaseSettings.FeatureCollectionName);
        }

        public async Task CreateFeatureAsync(CreateFeatureDto createFeatureDto)
        {
           await _featureCollection.InsertOneAsync(_mapper.Map<Feature>(createFeatureDto));
        }

        public async Task DeleteFeatureAsync(string id)
        {
            await _featureCollection.FindOneAndDeleteAsync(x => x.FeatureId == id);
        }

        public async Task<List<ResultFeatureDto>> GetAllFeatureAsync()
        {
            var values= await _featureCollection.Find(_=>true).ToListAsync();
            return _mapper.Map<List<ResultFeatureDto>>(values);
        }

        public async Task<ResultFeatureDto> GetByIdFeatureAsync(string id)
        {
            var value = await _featureCollection.Find(x=> x.FeatureId==id).FirstOrDefaultAsync();
            return _mapper.Map<ResultFeatureDto>(value);
        }

        public async Task UpdateFeatureAsync(UpdateFeatureDto updateFeatureDto)
        {
            var value = _mapper.Map<Feature>(updateFeatureDto);
            await _featureCollection.FindOneAndReplaceAsync(x => x.FeatureId == value.FeatureId, value);
        }
    }
}
