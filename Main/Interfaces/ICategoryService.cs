using EntityModels.Models;

namespace Main.Interfaces;

public interface ICategoryService
{
    IEnumerable<Category> GetCategories();
}
