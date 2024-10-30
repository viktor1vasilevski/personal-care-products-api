using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Constants;
using Main.DTOs.Category;
using Main.DTOs.Subcategory;
using Main.Enums;
using Main.Interfaces;
using Main.Responses;
using Microsoft.EntityFrameworkCore;

namespace Main.Services;

public class SubcategoryService : ISubcategoryService
{
    private IUnitOfWork<LibraryDbContext> _uow;
    private IGenericRepository<Subcategory> _subcategoryRepository;
    public SubcategoryService(IUnitOfWork<LibraryDbContext> uow)
    {
        _uow = uow;
        _subcategoryRepository = _uow.GetGenericRepository<Subcategory>();
    }


    public QueryResponse<List<SubcategoryDTO>> GetSubcategories(int? skip, int? take)
    {
        try
        {
            var subcategories = _subcategoryRepository.GetAsQueryable(null, null, null);

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

    public SingleResponse<SubcategoryDTO> GetSubcategoryById(Guid id)
    {
        try
        {
            var status = _subcategoryRepository.Exists(x => x.Id == id);
            if (status)
            {
                var subcategory = _subcategoryRepository.Get(x => x.Id == id).FirstOrDefault();

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

}
