using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Constants;
using Main.DTOs.Category;
using Main.DTOs.Responses;
using Main.Interfaces;
using Main.Responses;
using Microsoft.EntityFrameworkCore;

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

    public ApiResponse<List<CategoryDTO>> GetCategories(int? skip, int? take)
    {
        try
        {
            var categories = _categoryRepository.GetAsQueryable(null, null, null);

            var totalCount = categories.Count();

            if (skip.HasValue)
                categories = categories.Skip(skip.Value);

            if (take.HasValue)
                categories = categories.Take(take.Value);

            var categoriesDTO = categories.Select(x => new CategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy
            }).ToList();

            return new ApiResponse<List<CategoryDTO>>() 
            { 
                Success = true, 
                Data = categoriesDTO,
                TotalCount = totalCount
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CategoryDTO>>() 
            { 
                Success = false, 
                Message = CategoryConstants.ERROR_RETRIEVING_CATEGORIES, 
                ExceptionMessage = ex.Message 
            };
        }
    }

    public SingleResponse<CategoryDTO> CreateCategory(CreateCategoryDTO request)
    {
        try
        {
            if (!String.IsNullOrEmpty(request.Name))
            {
                string name = request.Name;
                var status = _categoryRepository.Exists(x => x.Name.ToLower() == name.ToLower());
                if (status)
                    return new SingleResponse<CategoryDTO>() { Success = false, Message = CategoryConstants.CATEGORY_EXISTS };

                var entity = _categoryRepository.Insert(new Category { Name = name });
                _uow.SaveChanges();

                return new SingleResponse<CategoryDTO>()
                {
                    Success = true,
                    Message = CategoryConstants.CATEGORY_SUCCESSFULLY_CREATED,
                    Data = new CategoryDTO
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Created = entity.Created,
                        CreatedBy = entity.CreatedBy,
                        LastModified = entity.LastModified,
                        LastModifiedBy = entity.LastModifiedBy
                    },
                };
            }
            else
            {
                return new SingleResponse<CategoryDTO>() 
                { 
                    Success = false, 
                    Message = CategoryConstants.CATEGORY_NAME_IS_EMPTY 
                };
            }
            
        }
        catch (Exception ex)
        {
            return new SingleResponse<CategoryDTO> 
            { 
                Success = false,
                Message = CategoryConstants.ERROR_CREATING_CATEGORY,
                ExceptionMessage = ex.Message
            };
        }
    }

    public SingleResponse<CategoryDTO> DeleteCategory(Guid id)
    {
        try
        {
            var status = _categoryRepository.Exists(x => x.Id == id);
            if (status)
            {
                var category = _categoryRepository.GetAsQueryable(x => x.Id == id).Include(x => x.Subcategory).FirstOrDefault();

                if (category.Subcategory.Any())
                {
                    return new SingleResponse<CategoryDTO>()
                    {
                        Success = false,
                        Message = CategoryConstants.CATEGORY_HAS_LINKED_SUBCATEGORIES,
                    };
                }

                var entity = _categoryRepository.Delete(id);
                _uow.SaveChanges();

                return new SingleResponse<CategoryDTO>()
                {
                    Success = true,
                    Message = CategoryConstants.CATEGORY_SUCCESSFULLY_DELETED,
                    Data = new CategoryDTO
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Created = entity.Created,
                        CreatedBy = entity.CreatedBy,
                        LastModified = entity.LastModified,
                        LastModifiedBy = entity.LastModifiedBy
                    }
                };
            }
            else
            {
                return new SingleResponse<CategoryDTO>() 
                { 
                    Success = false, 
                    Message = CategoryConstants.CATEGORY_DOESNT_EXIST 
                };
            }               
        }
        catch (Exception ex)
        {
            return new SingleResponse<CategoryDTO>() 
            { 
                Success = false,
                Message = CategoryConstants.ERROR_DELETING_CATEGORY,
                ExceptionMessage = ex.Message
            };

        }
    }
}
