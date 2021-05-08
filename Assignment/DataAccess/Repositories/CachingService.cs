using Assignment.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Assignment.DataAccess.Repositories
{
    public class CachingService : ICachingService
    {

        public IDatabase database;
        private readonly IConfiguration _config;

         
        public CachingService(IConfiguration config)
        {
            _config = config;
            var connection = config.GetConnectionString("cachedb");
            var multiplex = ConnectionMultiplexer.Connect(connection);
            
            database = multiplex.GetDatabase();
        }


        public void DeleteCategory(string Name)
        {
            throw new NotImplementedException();
        }

        public List<CategorySaving> GetCategories()
        {
            if (database.KeyExists("categories"))
            {


                var result = JsonConvert.DeserializeObject<List<CategorySaving>>(
                  database.StringGet("categories")
                     );

                return result;

            }
            else return new List<CategorySaving>();
        }

        public void UpsertCategorySaving(CategorySaving c)
        {
            List<CategorySaving> savedCategories = GetCategories();
            Console.WriteLine(c.CategoryId);
            if (savedCategories.Count(x=>x.CategoryId == c.CategoryId) > 0)
            {
                var originalCategory = savedCategories.SingleOrDefault(x => x.CategoryId == c.CategoryId);
                originalCategory.Name= c.Name;
                originalCategory.Price = c.Price;

            }
            else
            {
                c.CategoryId = savedCategories.Count() + 1;
                savedCategories.Add(c);
            }

            database.StringSet("categories", JsonConvert.SerializeObject(savedCategories));


        }

        public CategorySaving GetCategory(int id)
        {
            List<CategorySaving> savedCategories = GetCategories();
            if (savedCategories.Count(x => x.CategoryId == id) > 0)
            {
                var oc = savedCategories.SingleOrDefault(x => x.CategoryId == id);
                return oc;
            }
            else
            {
                return null;
            }

        }
    }
}
