using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class ContextSeed
    {
        public static void SeedData<TEntity>(IMongoCollection<TEntity> collection, string fileName)
            where TEntity : BaseEntity
        {
            bool check = collection.Find(b => true).Any();
            string path = Path.Combine("Data", "SeedData", $"{fileName}.json");

            if (!check)
            {
                var data = File.ReadAllText(path);
                var deserializedData = JsonSerializer.Deserialize < List<TEntity>>(data);

                if(deserializedData != null)
                {
                    foreach(var item in deserializedData)
                    {
                        collection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
