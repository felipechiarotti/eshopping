using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ProductBrand> Brands { get; }
        public IMongoCollection<ProductType> Types { get; }

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("MongoDb:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDb:DatabaseName"));

            var collectionName = configuration.GetValue<string>("MongoDb:CollectionName");
            var typesCollection = configuration.GetValue<string>("MongoDb:TypesCollection");
            var brandsCollection = configuration.GetValue<string>("MongoDb:BrandsCollection");

            Products = database.GetCollection<Product>(collectionName);
            Brands = database.GetCollection<ProductBrand>(brandsCollection);
            Types = database.GetCollection<ProductType>(typesCollection);

            ContextSeed.SeedData(Products, collectionName.ToLower());
            ContextSeed.SeedData(Brands, brandsCollection.ToLower());
            ContextSeed.SeedData(Types, typesCollection.ToLower());
        }
    }
}
