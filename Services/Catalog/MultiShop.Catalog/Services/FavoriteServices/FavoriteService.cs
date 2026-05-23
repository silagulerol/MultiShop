using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.FavoriteDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FavoriteServices
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IMongoCollection<Favorite> _favoriteCollection;
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;

        public FavoriteService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _favoriteCollection = database.GetCollection<Favorite>(databaseSettings.FavoriteCollectionName);
            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _mapper = mapper;
        }

        public async Task<List<ResultFavoriteDto>> GetAllFavoriteAsync()
        {
            var values = await _favoriteCollection
                .Find(_ => true)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            await ResolveProductsAsync(values);
            return _mapper.Map<List<ResultFavoriteDto>>(values);
        }

        public async Task CreateFavoriteAsync(CreateFavoriteDto createFavoriteDto)
        {
            var existingFavorite = await _favoriteCollection
                .Find(x => x.UserId == createFavoriteDto.UserId && x.ProductId == createFavoriteDto.ProductId)
                .FirstOrDefaultAsync();

            if (existingFavorite is not null)
            {
                return;
            }

            var value = _mapper.Map<Favorite>(createFavoriteDto);
            value.CreatedDate = createFavoriteDto.CreatedDate == default ? DateTime.UtcNow : createFavoriteDto.CreatedDate;
            await _favoriteCollection.InsertOneAsync(value);
            await SyncProductFavoriteCountAsync(value.ProductId);
        }

        public async Task UpdateFavoriteAsync(UpdateFavoriteDto updateFavoriteDto)
        {
            var duplicateFavorite = await _favoriteCollection
                .Find(x => x.FavoriteId != updateFavoriteDto.FavoriteId &&
                           x.UserId == updateFavoriteDto.UserId &&
                           x.ProductId == updateFavoriteDto.ProductId)
                .FirstOrDefaultAsync();

            if (duplicateFavorite is not null)
            {
                return;
            }

            var oldFavorite = await _favoriteCollection
                .Find(x => x.FavoriteId == updateFavoriteDto.FavoriteId)
                .FirstOrDefaultAsync();

            var value = _mapper.Map<Favorite>(updateFavoriteDto);
            value.CreatedDate = updateFavoriteDto.CreatedDate == default ? DateTime.UtcNow : updateFavoriteDto.CreatedDate;
            await _favoriteCollection.ReplaceOneAsync(x => x.FavoriteId == value.FavoriteId, value);

            if (oldFavorite is not null && oldFavorite.ProductId != value.ProductId)
            {
                await SyncProductFavoriteCountAsync(oldFavorite.ProductId);
            }

            await SyncProductFavoriteCountAsync(value.ProductId);
        }

        public async Task DeleteFavoriteAsync(string id)
        {
            var favorite = await _favoriteCollection.Find(x => x.FavoriteId == id).FirstOrDefaultAsync();
            await _favoriteCollection.DeleteOneAsync(x => x.FavoriteId == id);

            if (favorite is not null)
            {
                await SyncProductFavoriteCountAsync(favorite.ProductId);
            }
        }

        public async Task<GetByIdFavoriteDto> GetByIdFavoriteAsync(string id)
        {
            var value = await _favoriteCollection.Find(x => x.FavoriteId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdFavoriteDto>(value);
        }

        public async Task<List<ResultFavoriteDto>> GetFavoritesByUserIdAsync(string userId)
        {
            var values = await _favoriteCollection
                .Find(x => x.UserId == userId)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            await ResolveProductsAsync(values);
            return _mapper.Map<List<ResultFavoriteDto>>(values);
        }

        public async Task<bool> IsProductFavoritedAsync(string userId, string productId)
        {
            return await _favoriteCollection
                .Find(x => x.UserId == userId && x.ProductId == productId)
                .AnyAsync();
        }

        public async Task ToggleFavoriteAsync(string userId, string productId)
        {
            var favorite = await _favoriteCollection
                .Find(x => x.UserId == userId && x.ProductId == productId)
                .FirstOrDefaultAsync();

            if (favorite is not null)
            {
                await _favoriteCollection.DeleteOneAsync(x => x.FavoriteId == favorite.FavoriteId);
                await SyncProductFavoriteCountAsync(productId);
                return;
            }

            await _favoriteCollection.InsertOneAsync(new Favorite
            {
                UserId = userId,
                ProductId = productId,
                CreatedDate = DateTime.UtcNow
            });

            await SyncProductFavoriteCountAsync(productId);
        }

        public async Task<int> GetFavoriteCountByProductIdAsync(string productId)
        {
            var count = await _favoriteCollection.CountDocumentsAsync(x => x.ProductId == productId);
            return (int)count;
        }

        private async Task ResolveProductsAsync(List<Favorite> favorites)
        {
            foreach (var item in favorites)
            {
                var product = await _productCollection.Find(x => x.ProductId == item.ProductId).FirstOrDefaultAsync();

                if (product is not null)
                {
                    item.Product = product;
                }
            }
        }

        private async Task SyncProductFavoriteCountAsync(string productId)
        {
            var count = await GetFavoriteCountByProductIdAsync(productId);
            var update = Builders<Product>.Update.Set(x => x.FavoriteCount, count);
            await _productCollection.UpdateOneAsync(x => x.ProductId == productId, update);
        }
    }
}
