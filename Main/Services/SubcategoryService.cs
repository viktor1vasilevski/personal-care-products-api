using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Interfaces;

namespace Main.Services;

public class SubcategoryService : ISubcategoryService
{
    private IUnitOfWork<LibraryDbContext> _uow;
    private IGenericRepository<Subcategory> _subCategoryRepository;
    public SubcategoryService(IUnitOfWork<LibraryDbContext> uow)
    {
        _uow = uow;
        _subCategoryRepository = _uow.GetGenericRepository<Subcategory>();
    }

    public IEnumerable<Subcategory> GetSubcategories()
    {
        throw new NotImplementedException();
    }
}
