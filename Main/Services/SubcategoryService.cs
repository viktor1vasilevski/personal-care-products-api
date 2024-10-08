using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.DTOs.Product;
using Main.DTOs.Responses;
using Main.DTOs.Subcategory;
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

    public ApiResponse<List<SubcategoryDTO>> GetSubcategories(int? skip, int? take)
    {
        try
        {
            var subcategories = _subCategoryRepository.GetAsQueryable(null, null, null);

            if (skip.HasValue)
                subcategories = subcategories.Skip(skip.Value);

            if (take.HasValue)
                subcategories = subcategories.Take(take.Value);

            var subcategoriesDTO = subcategories.Select(x => new SubcategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy
            }).ToList();

            return new ApiResponse<List<SubcategoryDTO>>() { Data = subcategoriesDTO, Success = true };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<SubcategoryDTO>>() { Success = false, Message = "Se desi zbunka", ExceptionMessage = ex.Message };
        }
    }
}
