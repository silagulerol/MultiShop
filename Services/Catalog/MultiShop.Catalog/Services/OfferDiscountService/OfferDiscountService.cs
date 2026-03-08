using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.OfferDiscountDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.OfferDiscountService
{
    public class OfferDiscountService : IOfferDiscountService
    {
        private readonly IMongoCollection<OfferDiscount> _offerDiscountdCollection;
        private readonly IMapper _mapper;

        public OfferDiscountService(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            _mapper = mapper;
            var MongoClient = new MongoClient(_databaseSettings.ConnectionString);
            var database = MongoClient.GetDatabase(_databaseSettings.DatabaseName);
            _offerDiscountdCollection = database.GetCollection<OfferDiscount>(_databaseSettings.OfferDiscountCollectionName);
        }

        public async Task CreateOfferDiscountAsync(CreateOfferDiscountDto createOfferDiscountDto)
        {
            await _offerDiscountdCollection.InsertOneAsync(_mapper.Map<OfferDiscount>(createOfferDiscountDto));
        }

        public async Task DeleteOfferDiscountAsync(string id)
        {
            await _offerDiscountdCollection.FindOneAndDeleteAsync(x => x.OfferDiscountId == id);
        }

        public async Task<List<ResultOfferDiscountDto>> GetAllOfferDiscountAsync()
        {
            var values = await _offerDiscountdCollection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultOfferDiscountDto>>(values);
        }

        public async Task<ResultOfferDiscountDto> GetByIdOfferDiscountAsync(string id)
        {
            var value = await _offerDiscountdCollection.Find(x => x.OfferDiscountId == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultOfferDiscountDto>(value);
        }

        public async Task UpdateOfferDiscountAsync(UpdateOfferDiscountDto updateOfferDiscountDto)
        {
            var value = _mapper.Map<OfferDiscount>(updateOfferDiscountDto);
            await _offerDiscountdCollection.FindOneAndReplaceAsync(x => x.OfferDiscountId == value.OfferDiscountId, value);
        }
    }
}
