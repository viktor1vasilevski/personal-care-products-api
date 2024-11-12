using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Constants;
using Main.DTOs.Category;
using Main.Enums;
using Main.Extensions;
using Main.Interfaces;
using Main.Requests;
using Main.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    public QueryResponse<List<CategoryDTO>> GetCategories(CategoryRequest request)
    {
        try
        {
            var categories = _categoryRepository.GetAsQueryableWhereIf(c => c
                .WhereIf(!String.IsNullOrEmpty(request.Name), x => x.Name.ToLower().Contains(request.Name.ToLower())), 
                null, 
                null
                );

            if (!string.IsNullOrEmpty(request.Sort))
            {
                categories = request.Sort.ToLower() switch
                {
                    "asc" => categories.OrderBy(x => x.Created),
                    "desc" => categories.OrderByDescending(x => x.Created),
                    _ => categories.OrderByDescending(x => x.Created)
                };
            }

            var totalCount = categories.Count();

            if (request.Skip.HasValue)
                categories = categories.Skip(request.Skip.Value);

            if (request.Take.HasValue)
                categories = categories.Take(request.Take.Value);

            var categoriesDTO = categories.Select(x => new CategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy
            }).ToList();

            return new QueryResponse<List<CategoryDTO>>() 
            { 
                Success = true, 
                Data = categoriesDTO,
                TotalCount = totalCount,
                NotificationType = NotificationType.Success,
            };
        }
        catch (Exception ex)
        {
            return new QueryResponse<List<CategoryDTO>>() 
            { 
                Success = false, 
                Message = CategoryConstants.ERROR_RETRIEVING_CATEGORIES, 
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }

    public SingleResponse<CategoryDTO> CreateCategory(CreateUpdateCategoryDTO request)
    {
        try
        {
            if (!String.IsNullOrEmpty(request.Name))
            {
                string name = request.Name;
                var status = _categoryRepository.Exists(x => x.Name.ToLower() == name.ToLower());
                if (status)
                    return new SingleResponse<CategoryDTO>() 
                    { 
                        Success = false, 
                        Message = CategoryConstants.CATEGORY_EXISTS,
                        NotificationType = NotificationType.Info
                    };

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
                    NotificationType = NotificationType.Success
                };
            }
            else
            {
                return new SingleResponse<CategoryDTO>() 
                { 
                    Success = false, 
                    Message = CategoryConstants.CATEGORY_NAME_IS_EMPTY,
                    NotificationType = NotificationType.Info
                };
            }
            
        }
        catch (Exception ex)
        {
            return new SingleResponse<CategoryDTO> 
            { 
                Success = false,
                Message = CategoryConstants.ERROR_CREATING_CATEGORY,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }

    public SingleResponse<CategoryDTO> DeleteCategory(Guid id)
    {
        try
        {
            if (_categoryRepository.Exists(x => x.Id == id))
            {
                var category = _categoryRepository.GetAsQueryable(x => x.Id == id).Include(x => x.Subcategory).FirstOrDefault();

                if (category.Subcategory.Any())
                {
                    return new SingleResponse<CategoryDTO>()
                    {
                        Success = false,
                        Message = CategoryConstants.CATEGORY_HAS_LINKED_SUBCATEGORIES,
                        NotificationType = NotificationType.Info
                    };
                }

                var entity = _categoryRepository.Delete(id);
                _uow.SaveChanges();

                return new SingleResponse<CategoryDTO>()
                {
                    Success = true,
                    Message = CategoryConstants.CATEGORY_SUCCESSFULLY_DELETED,
                    NotificationType = NotificationType.Success,
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
                    Message = CategoryConstants.CATEGORY_DOESNT_EXIST,
                    NotificationType = NotificationType.Info
                };
            }               
        }
        catch (Exception ex)
        {
            return new SingleResponse<CategoryDTO>() 
            { 
                Success = false,
                Message = CategoryConstants.ERROR_DELETING_CATEGORY,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }

    public SingleResponse<CategoryDTO> GetCategoryById(Guid id)
    {
        try
        {
            if (_categoryRepository.Exists(x => x.Id == id))
            {
                var category = _categoryRepository.GetAsQueryable(x => x.Id == id).Include(x => x.Subcategory).FirstOrDefault();

                return new SingleResponse<CategoryDTO>()
                {
                    Success = true,
                    Message = CategoryConstants.CATEGORY_SUCCESSFULLY_RETRIVED,
                    NotificationType = NotificationType.Success,
                    Data = new CategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Created = category.Created,
                        CreatedBy = category.CreatedBy,
                        LastModified = category.LastModified,
                        LastModifiedBy = category.LastModifiedBy,
                        Subcategories = category.Subcategory?.Select(sc => sc.Name).ToList() ?? new List<string>()
                    }
                };
            }
            else
            {
                return new SingleResponse<CategoryDTO>()
                {
                    Success = false,
                    Message = CategoryConstants.CATEGORY_DOESNT_EXIST,
                    NotificationType = NotificationType.Info
                };
            }
        }
        catch (Exception ex)
        {
            return new SingleResponse<CategoryDTO>()
            {
                Success = false,
                Message = CategoryConstants.CATEGORY_GET_BY_ID_ERROR,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }

    public SingleResponse<CategoryDTO> UpdateCategory(Guid id, CreateUpdateCategoryDTO request)
    {
        try
        {
            if (_categoryRepository.Exists(x => x.Id == id))
            {
                var category = _categoryRepository.GetByID(id);
                category.Name = request.Name;
                _categoryRepository.Update(category);
                _uow.SaveChanges();

                return new SingleResponse<CategoryDTO>()
                {
                    Success = true,
                    Data = new CategoryDTO()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        LastModified = category.LastModified,
                        LastModifiedBy = category.LastModifiedBy,
                        Created = category.Created,
                        CreatedBy = category.CreatedBy,
                    },
                    Message = CategoryConstants.CATEGORY_SUCCESSFULLY_UPDATED,
                    NotificationType = NotificationType.Success
                };
            }
            else
            {
                return new SingleResponse<CategoryDTO>()
                {
                    Success = false,
                    Message = CategoryConstants.CATEGORY_DOESNT_EXIST,
                    NotificationType = NotificationType.Info
                };
            }
        }
        catch (Exception ex)
        {
            return new SingleResponse<CategoryDTO>()
            {
                Success = false,
                Message = CategoryConstants.ERROR_UPDATE_CATEGORY,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }

    public QueryResponse<List<CategoryDropdownDTO>> GetCategoriesForDropdown(CategoryForDropdownRequest request)
    {
        throw new NotImplementedException();
    }
}
