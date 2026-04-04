using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ContactDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ContactService
{
    public class ContactService : IContactService
    {
        private readonly IMongoCollection<Contact> _contactCollection;
        private readonly IMapper _mapper;

        public ContactService(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            var client =  new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _contactCollection = database.GetCollection<Contact>(_databaseSettings.ContactCollectionName);
            _mapper = mapper;
        }

        public async Task AddContactAsync(CreateContactDto createContactDto)
        {
            var value = _mapper.Map<Contact>(createContactDto);
            await _contactCollection.InsertOneAsync(value);
        }

        public async Task DeleteContactAsync(string id)
        {
            await _contactCollection.DeleteOneAsync(x => x.ContactId == id);
        }

        public Task UpdateContactAsync(UpdateContactDto updateContactDto)
        {
            var value = _mapper.Map<Contact>(updateContactDto);
            return _contactCollection.ReplaceOneAsync(x => x.ContactId == updateContactDto.ContactId, value);
        }

        public async Task<List<ResultContactDto>> ResultContactsAsync()
        {
            var values= await _contactCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultContactDto>>(values);
        }

        public async Task<ResultContactDto> GetContactByIdAsync(string id)
        {
            var value = await _contactCollection.Find(x => x.ContactId == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultContactDto>(value);
        }
       
    }
}
