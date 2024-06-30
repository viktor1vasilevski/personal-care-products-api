using EntityModels.Models;

namespace Main.Interfaces;

public interface ISubcategoryService
{
    IEnumerable<Subcategory> GetSubcategories();
}
