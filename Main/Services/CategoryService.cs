using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Interfaces;

namespace Main.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork<LibraryDbContext> _uow;
    private readonly IGenericRepository<Category> _categoryRepository;
    public CategoryService(IUnitOfWork<LibraryDbContext> uow)
    {
        _uow = uow;
        _categoryRepository = _uow.GetGenericRepository<Category>();
    }
    public IEnumerable<Category> GetCategories()
    {
        throw new NotImplementedException();
    }
}
