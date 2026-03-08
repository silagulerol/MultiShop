using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.SpecialOfferDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.SpecialOfferService
{
    public class SpecialOfferService : ISpecialOfferService
    {
        private readonly IMongoCollection<SpecialOffer> _specialOfferCollection;
        private readonly IMapper _mapper;

        public SpecialOfferService( IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            var MongoClient = new MongoClient(_databaseSettings.ConnectionString);
            var database = MongoClient.GetDatabase(_databaseSettings.DatabaseName);
            _specialOfferCollection = database.GetCollection<SpecialOffer>(_databaseSettings.SpecialOfferCollectionName);
            _mapper = mapper;
        }

        public async Task DeleteSpecialOfferAsync(string id)
        {
            await _specialOfferCollection.FindOneAndDeleteAsync(x => x.SpecialOfferId == id);
        }

        public async Task<List<ResultSpecialOfferDto>> GetAllSpecialOffersAsync()
        {
            var values = await _specialOfferCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultSpecialOfferDto>>(values);
        }

        public async Task<ResultSpecialOfferDto> GetSpecialOfferByIdAsync(string id)
        {
            var value = await _specialOfferCollection.Find(x => x.SpecialOfferId == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultSpecialOfferDto>(value);
        }

        public async Task InsertSpecialOfferAsync(CreateSpecialOfferDto createSpecialOfferDto)
        {
            await _specialOfferCollection.InsertOneAsync(_mapper.Map<SpecialOffer>(createSpecialOfferDto));
        }

        public async Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            var value = _mapper.Map<SpecialOffer>(updateSpecialOfferDto);
            await _specialOfferCollection.FindOneAndReplaceAsync(x => x.SpecialOfferId == value.SpecialOfferId, value);
        }
        
        public Task FeatureSliderChangeToFalseAsync(string id)
        {
            throw (new NotImplementedException());

        }

        public Task FeatureSliderChangeToTrueAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
