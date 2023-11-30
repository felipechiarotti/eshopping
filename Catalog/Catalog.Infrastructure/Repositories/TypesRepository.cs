using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class TypesRepository : ITypesRepository
    {
        private readonly ICatalogContext _catalogContext;

        public TypesRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<ProductType> CreateAsync(ProductType entity)
        {
            await _catalogContext.Types.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<ProductType>.Filter.Eq(p => p.Id, id);
            var deleteResult = await _catalogContext.Types.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductType>> GetAllAsync()
        {
            var types = await _catalogContext.Types.FindAsync(p => true);
            return await types.ToListAsync();
        }

        public async Task<ProductType> GetByIdAsync(string id)
        {
            var types = await _catalogContext.Types.FindAsync(p => p.Id == id);
            return await types.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(ProductType entity)
        {
            var replaceResult = await _catalogContext.Types.ReplaceOneAsync(p => p.Id == entity.Id, entity);
            return replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0;
        }
    }
}
