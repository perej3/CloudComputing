using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Interfaces
{
    public interface ICachingService
    {
        List<CategorySaving> GetCategories();

        CategorySaving GetCategory(int id);
        void UpsertCategorySaving(CategorySaving c);

        void DeleteCategory(string Name);

    }

    public class CategorySaving
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
       
}
