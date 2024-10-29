using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Constants;
using Main.DTOs.Subcategory;
using Main.Enums;
using Main.Interfaces;
using Main.Responses;

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


    public QueryResponse<List<SubcategoryDTO>> GetSubcategories(int? skip, int? take)
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

            return new QueryResponse<List<SubcategoryDTO>>() 
            { 
                Success = true,
                Data = subcategoriesDTO
            };
        }
        catch (Exception ex)
        {
            return new QueryResponse<List<SubcategoryDTO>>() 
            { 
                Success = false, 
                Message = SubcategoryConstants.ERROR_RETRIEVING_SUBCATEGORIES, 
                ExceptionMessage = ex.Message 
            };
        }
    }

    public SingleResponse<SubcategoryDTO> GetByIdSubcategory(Guid id)
    {
        try
        {
            var status = _subCategoryRepository.Exists(x => x.Id == id);
            if (status)
            {
                var subcategory = _subCategoryRepository.Get(x => x.Id == id).FirstOrDefault();

                return new SingleResponse<SubcategoryDTO>()
                {
                    Success = true,
                    Message = SubcategoryConstants.SUBCATEGORY_SUCCESSFULLY_RETRIVED,
                    NotificationType = NotificationType.Success,
                    Data = new SubcategoryDTO
                    {
                        Id = subcategory.Id,
                        Name = subcategory.Name,
                        Created = subcategory.Created,
                        CreatedBy = subcategory.CreatedBy,
                        LastModified = subcategory.LastModified,
                        LastModifiedBy = subcategory.LastModifiedBy,
                        //Subcategories = category.Subcategory?.Select(sc => sc.Name).ToList() ?? new List<string>()
                    }
                };
            }
            else
            {
                return new SingleResponse<SubcategoryDTO>()
                {
                    Success = false,
                    Message = SubcategoryConstants.SUBCATEGORY_DOESNT_EXIST,
                    NotificationType = NotificationType.Info
                };
            }
        }
        catch (Exception ex)
        {
            return new SingleResponse<SubcategoryDTO>()
            {
                Success = false,
                Message = SubcategoryConstants.ERROR_GET_SUBCATEGORY_BY_ID,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }
}
