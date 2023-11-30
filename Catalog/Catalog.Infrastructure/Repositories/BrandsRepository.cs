using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandsRepository : IBrandRepository
    {
        private readonly ICatalogContext _catalogContext;

        public BrandsRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<ProductBrand> CreateAsync(ProductBrand entity)
        {
            await _catalogContext.Brands.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<ProductBrand>.Filter.Eq(p => p.Id, id);
            var deleteResult = await _catalogContext.Brands.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllAsync()
        {
            var brands = await _catalogContext.Brands.FindAsync(p => true);
            return await brands.ToListAsync();
        }

        public async Task<ProductBrand> GetByIdAsync(string id)
        {
            var brands = await _catalogContext.Brands.FindAsync(p => p.Id == id);
            return await brands.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(ProductBrand entity)
        {
            var replaceResult = await _catalogContext.Brands.ReplaceOneAsync(p => p.Id == entity.Id, entity);
            return replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0;
        }
    }
}
