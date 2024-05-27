using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public static class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            bool checkType = typeCollection.Find(b => true).Any();
            //string path = Path.Combine("Data", "SeedData", "types.json");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SeedData", "types.json");
            if (!checkType)
            {
                var typeData = File.ReadAllText(path);
                var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                if (types != null)
                {
                    foreach (var item in types)
                    {
                        typeCollection.InsertOne(item);
                    }
                }
            }
        }
    }
}
