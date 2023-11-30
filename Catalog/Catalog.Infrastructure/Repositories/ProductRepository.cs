using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            await _catalogContext.Products.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var deleteResult = await _catalogContext.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _catalogContext.Products.FindAsync(p => true);
            return await products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByBrandAsync(string brand)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, brand);
            var products = await _catalogContext.Products.FindAsync(filter);
            return await products.ToListAsync() ;
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            var product = await _catalogContext.Products.FindAsync(p => p.Id == id);
            return await product.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            var products = await _catalogContext
                .Products
                .FindAsync(filter);
            return await products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByTypeAsync(string type)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Types.Name, type);
            var products = await _catalogContext
                .Products
                .FindAsync(filter);
            return await products.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Product entity)
        {
            var replaceResult = await _catalogContext.Products.ReplaceOneAsync(p => p.Id == entity.Id, entity);
            return replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0;
        }
    }
}
