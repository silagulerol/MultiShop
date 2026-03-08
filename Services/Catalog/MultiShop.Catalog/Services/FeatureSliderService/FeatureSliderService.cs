using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.FeatureSliderDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FeatureSliderService
{
    public class FeatureSliderService : IFeatureSliderService
    {
        private readonly IMongoCollection<FeatureSlider> _featureSliderCollection;
        private readonly IMapper _mapper;

        public FeatureSliderService(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            var MongoClient = new MongoClient(_databaseSettings.ConnectionString);
            var database = MongoClient.GetDatabase(_databaseSettings.DatabaseName);
            _featureSliderCollection = database.GetCollection<FeatureSlider>(_databaseSettings.FeatureSliderCollectionName);
            _mapper = mapper;
        }

        public async Task DeleteFeatureSliderAsync(string id)
        {
            await _featureSliderCollection.FindOneAndDeleteAsync(x => x.FeatureSliderId == id);
        }

        public Task FeatureSliderChangeToFalseAsync(string id)
        {
            throw(new NotImplementedException());
            
        }

        public Task FeatureSliderChangeToTrueAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResultFeatureSliderDto>> GetAllFeatureSlidersAsync()
        {
            var values= await _featureSliderCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultFeatureSliderDto>>(values);
        }

        public async Task<ResultFeatureSliderDto> GetFeatureSliderByIdAsync(string id)
        {
            var value = await _featureSliderCollection.Find(x => x.FeatureSliderId == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultFeatureSliderDto>(value);
        }

        public async Task InsertFeatureSliderAsync(CreateFeatureSliderDto createFeatureSliderDto)
        {
            await _featureSliderCollection.InsertOneAsync(_mapper.Map<FeatureSlider>(createFeatureSliderDto));
        }

        public async Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            var value = _mapper.Map<FeatureSlider>(updateFeatureSliderDto);
            await _featureSliderCollection.FindOneAndReplaceAsync(x => x.FeatureSliderId == value.FeatureSliderId, value);
        }
    }
}
