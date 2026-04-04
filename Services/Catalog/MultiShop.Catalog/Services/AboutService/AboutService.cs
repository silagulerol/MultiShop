using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.AboutDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.AboutService
{
    public class AboutService : IAboutService
    {
        private readonly IMongoCollection<About> _aboutCollection;
        private readonly IMapper _mapper;

        public AboutService(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            _mapper = mapper;
            var MongoClient = new MongoClient(_databaseSettings.ConnectionString);
            var database = MongoClient.GetDatabase(_databaseSettings.DatabaseName);
            _aboutCollection = database.GetCollection<About>(_databaseSettings.AboutCollectionName);
        }
        public async Task CreateAboutAsync(CreateAboutDto createAboutDto)
        {
            await _aboutCollection.InsertOneAsync(_mapper.Map<About>(createAboutDto));
        }

        public async Task DeleteAboutAsync(string id)
        {
            await _aboutCollection.FindOneAndDeleteAsync(x => x.AboutId == id);
        }

        public async Task<List<ResultAboutDto>> GetAllAboutAsync()
        {
            var values = await _aboutCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultAboutDto>>(values);
        }

        public async Task<ResultAboutDto> GetByIdAboutAsync(string id)
        {
            var value = await _aboutCollection.Find(x => x.AboutId == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultAboutDto>(value);
        }

        public async Task UpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            var value = _mapper.Map<About>(updateAboutDto);
            await _aboutCollection.FindOneAndReplaceAsync(x => x.AboutId == value.AboutId, value);
        }
    }
}
