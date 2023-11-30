using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByNameAsync(string name);
        Task<IEnumerable<Product>> GetByBrandAsync(string brand);
        Task<IEnumerable<Product>> GetByTypeAsync(string type);
    }
}
