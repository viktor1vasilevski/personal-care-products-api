using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Constants;
using Main.DTOs.Subcategory;
using Main.Enums;
using Main.Interfaces;
using Main.Requests;
using Main.Responses;
using Microsoft.EntityFrameworkCore;
using Main.Extensions;

namespace Main.Services;

public class SubcategoryService : ISubcategoryService
{
    private IUnitOfWork<LibraryDbContext> _uow;
    private IGenericRepository<Subcategory> _subcategoryRepository;
    private IGenericRepository<Category> _categoryRepository;
    public SubcategoryService(IUnitOfWork<LibraryDbContext> uow)
    {
        _uow = uow;
        _subcategoryRepository = _uow.GetGenericRepository<Subcategory>();
        _categoryRepository = _uow.GetGenericRepository<Category>();
    }


    public QueryResponse<List<SubcategoryDTO>> GetSubcategories(SubcategoryRequest request)
    {
        try
        {
            var subcategories = _subcategoryRepository.GetAsQueryableWhereIf(x =>
                x.WhereIf(!String.IsNullOrEmpty(request.Category), x => x.Category.Name.ToLower() == request.Category.ToLower())
                 .WhereIf(!String.IsNullOrEmpty(request.Name), x => x.Name.ToLower().Contains(request.Name.ToLower())),
                null,
                x => x.Include(x => x.Category));

            if (!string.IsNullOrEmpty(request.Sort))
            {
                subcategories = request.Sort.ToLower() switch
                {
                    "asc" => subcategories.OrderBy(x => x.Created),
                    "desc" => subcategories.OrderByDescending(x => x.Created),
                    _ => subcategories.OrderByDescending(x => x.Created)
                };
            }

            var totalCount = subcategories.Count();

            if (request.Skip.HasValue)
                subcategories = subcategories.Skip(request.Skip.Value);

            if (request.Take.HasValue)
                subcategories = subcategories.Take(request.Take.Value);

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
                Data = subcategoriesDTO,
                NotificationType = NotificationType.Success,
                TotalCount = totalCount
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

    public SingleResponse<SubcategoryDTO> GetSubcategoryById(Guid id)
    {
        try
        {
            var status = _subcategoryRepository.Exists(x => x.Id == id);
            if (status)
            {
                var subcategory = _subcategoryRepository.GetAsQueryable(x => x.Id == id, null,
                    x => x.Include(x => x.Products).Include(x => x.Category)).FirstOrDefault();

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
                        Products = subcategory.Products?.Select(p => p.Name).ToList() ?? new List<string>()
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


    public SingleResponse<SubcategoryDTO> DeleteSubcategory(Guid id)
    {
        try
        {
            if (_subcategoryRepository.Exists(x => x.Id == id))
            {
                var subcategory = _subcategoryRepository.GetAsQueryable(x => x.Id == id, null, x => x.Include(s => s.Category).Include(s => s.Products)).FirstOrDefault();

                if (!subcategory.Products.Any() && subcategory.Category.Name.Equals("UNCATEGORIZED"))
                {
                    var entity = _subcategoryRepository.Delete(id);
                    _uow.SaveChanges();

                    return new SingleResponse<SubcategoryDTO>()
                    {
                        Success = true,
                        Message = SubcategoryConstants.SUBCATEGORY_SUCCESSFULLY_DELETED,
                        NotificationType = NotificationType.Success,
                        Data = new SubcategoryDTO
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
                    return new SingleResponse<SubcategoryDTO>()
                    {
                        Success = false,
                        Message = SubcategoryConstants.SUBCATEGORY_HAS_LINKED_PRODUCTS_OR_CATEGORIES,
                        NotificationType = NotificationType.Info
                    };
                }           
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
                Message = SubcategoryConstants.ERROR_DELETING_SUBCATEGORY,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }

    public SingleResponse<SubcategoryDTO> CreateSubcategory(CreateUpdateSubcategoryDTO request)
    {
        try
        {
            var category = _categoryRepository.Exists(x => x.Id == request.CategoryId);
            if (category is false)
                return new SingleResponse<SubcategoryDTO> 
                { 
                    Success = false, 
                    Message = CategoryConstants.CATEGORY_DOESNT_EXIST,
                    NotificationType = NotificationType.Info
                };

            var suncategoryName = _subcategoryRepository.Exists(x => x.Name.ToLower() == request.Name.ToLower());
            if (suncategoryName)
                return new SingleResponse<SubcategoryDTO> 
                { 
                    Success = false, 
                    Message = SubcategoryConstants.SUBCATEGORY_EXISTS,
                    NotificationType = NotificationType.Info
                };

            var entity = new Subcategory()
            {
                Name = request.Name,
                CategoryId = request.CategoryId
            };

            _subcategoryRepository.Insert(entity);
            _uow.SaveChanges();

            return new SingleResponse<SubcategoryDTO>()
            {
                Success = true,
                Message = SubcategoryConstants.SUBCATEGORY_SUCCESSFULLY_CREATED,
                NotificationType = NotificationType.Success,
                Data = new SubcategoryDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Created = entity.Created,
                    CreatedBy = entity.CreatedBy
                }
            };
        }
        catch (Exception ex)
        {
            return new SingleResponse<SubcategoryDTO>()
            {
                Success = false,
                Message = SubcategoryConstants.ERROR_CREATING_SUBCATEGORY,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }
}
