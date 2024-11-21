using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Constants;
using Main.DTOs.Product;
using Main.Enums;
using Main.Extensions;
using Main.Interfaces;
using Main.Requests;
using Main.Responses;
using Microsoft.EntityFrameworkCore;

namespace Main.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork<LibraryDbContext> _uow;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Subcategory> _subcategoryRepository;
    private readonly IImageService _imageService;

    public ProductService(IUnitOfWork<LibraryDbContext> uow, IImageService imageService)
    {
        _uow = uow;
        _productRepository = _uow.GetGenericRepository<Product>();
        _subcategoryRepository = _uow.GetGenericRepository<Subcategory>();
        _imageService = imageService;
    }
    public QueryResponse<List<ProductDTO>> GetProducts(ProductRequest request)
    {
        try
        {
            var products = _productRepository.GetAsQueryableWhereIf(x => 
            x.WhereIf(!String.IsNullOrEmpty(request.CategoryId.ToString()), x => x.Subcategory.Category.Id == request.CategoryId)
             .WhereIf(!String.IsNullOrEmpty(request.SubcategoryId.ToString()), x => x.Subcategory.Id == request.SubcategoryId)
             .WhereIf(!String.IsNullOrEmpty(request.Name), x => x.Name.ToLower().Contains(request.Name.ToLower()))
             .WhereIf(!String.IsNullOrEmpty(request.Brand), x => x.Brand.ToLower().Contains(request.Brand.ToLower()))
             .WhereIf(!String.IsNullOrEmpty(request.Edition), x => x.Edition.ToLower().Contains(request.Edition.ToLower()))
             .WhereIf(!String.IsNullOrEmpty(request.Scent), x => x.Scent.ToLower().Contains(request.Scent.ToLower())), 
            null,
            x => x.Include(x => x.Subcategory).ThenInclude(sc => sc.Category));

            if (!string.IsNullOrEmpty(request.Sort))
            {
                products = request.Sort.ToLower() switch
                {
                    "asc" => products.OrderBy(x => x.Created),
                    "desc" => products.OrderByDescending(x => x.Created),
                    _ => products.OrderByDescending(x => x.Created)
                };
            }

            var totalCount = products.Count();

            if (request.Skip.HasValue)
                products = products.Skip(request.Skip.Value);

            if (request.Take.HasValue)
                products = products.Take(request.Take.Value);

            var productsDTO = products.Select(x => new ProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                Brand = x.Brand,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                UnitQuantity = x.UnitQuantity,
                Volume = x.Volume,
                Scent = x.Scent,
                ImageData = x.Image,
                ImageType = x.ImageType,
                Edition = x.Edition,
                Category = x.Subcategory.Category.Name,
                Subcategory = x.Subcategory.Name,
                SubcategoryId = x.SubcategoryId,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy
            }).ToList();

            return new QueryResponse<List<ProductDTO>>() 
            { 
                Success = true, 
                Data = productsDTO, 
                TotalCount = totalCount,
                NotificationType = NotificationType.Success
            };
        }
        catch (Exception ex)
        {
            return new QueryResponse<List<ProductDTO>>() 
            { 
                Success = false, 
                Message = ProductConstants.ERROR_RETRIEVING_PRODUCTS, 
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
        
    }
    public SingleResponse<ProductCreateDTO> CreateProduct(ProductCreateDTO model)
    {
        try
        {
            var subcategory = _subcategoryRepository.GetByID(model.SubcategoryId);
            if (subcategory is null)
                return new SingleResponse<ProductCreateDTO> { Success = false, Message = SubcategoryConstants.SUBCATEGORY_DOESNT_EXIST };

            string imageType = _imageService.ExtractImageType(model.Image);
            byte[] imageBytes = _imageService.ConvertBase64ToBytes(model.Image);

            var entity = new Product
            {
                Name = model.Name,
                Brand = model.Brand,
                Description = model.Description,
                Edition = model.Edition,
                Scent = model.Scent,
                Volume = model.Volume,
                UnitPrice = model.UnitPrice,
                UnitQuantity = model.UnitQuantity,
                Image = imageBytes,
                ImageType = imageType,
                SubcategoryId = model.SubcategoryId,
            };

            _productRepository.Insert(entity);
            _uow.SaveChanges();

            return new SingleResponse<ProductCreateDTO>
            {
                Success = true,
                Data = model,
                Message = ProductConstants.PRODUCT_SUCCESSFULLY_CREATED,
                NotificationType = NotificationType.Success

            };
        }
        catch (Exception ex)
        {
            return new SingleResponse<ProductCreateDTO>()
            {
                Success = false,
                Message = ProductConstants.PRODUCT_ERROR_CREATING,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }
    public SingleResponse<ProductDTO> GetProductById(Guid id)
    {
        try
        {
            if (_productRepository.Exists(x => x.Id == id))
            {
                var product = _productRepository.GetAsQueryable(x => x.Id == id, null, p => p.Include(x => x.Subcategory).ThenInclude(x => x.Category)).FirstOrDefault();

                return new SingleResponse<ProductDTO>()
                {
                    Success = true,
                    Message = ProductConstants.PRODUCT_SUCCESSFULLY_RETRIVED,
                    NotificationType = NotificationType.Success,
                    Data = new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Brand = product.Brand,
                        Description = product.Description,
                        UnitPrice = product.UnitPrice,
                        UnitQuantity = product.UnitQuantity,
                        Volume = product.Volume,
                        Scent = product.Scent,
                        Edition = product.Edition,
                        Created = product.Created,
                        CreatedBy = product.CreatedBy,
                        LastModified = product.LastModified,
                        LastModifiedBy = product.LastModifiedBy,
                        Subcategory = product.Subcategory?.Name,
                        Category = product.Subcategory?.Category?.Name
                    }
                };
            }
            else
            {
                return new SingleResponse<ProductDTO>()
                {
                    Success = false,
                    Message = ProductConstants.PRODUCT_GET_BY_ID_INFO,
                    NotificationType = NotificationType.Info
                };
            }
        }
        catch (Exception ex)
        {
            return new SingleResponse<ProductDTO>()
            {
                Success = false,
                Message = ProductConstants.PRODUCT_GET_BY_ID_ERROR,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }
    public SingleResponse<ProductDTO> DeleteProduct(Guid id)
    {
        try
        {
            if (_productRepository.Exists(x => x.Id == id))
            {
                var category = _productRepository.GetAsQueryable(x => x.Id == id).Include(x => x.Subcategory).FirstOrDefault();

                if (category.Subcategory.Name == "UNCATEGORIZED")
                {
                    _productRepository.Delete(category);
                    _uow.SaveChanges();

                    return new SingleResponse<ProductDTO>()
                    {
                        Success = true,
                        Message = ProductConstants.PRODUCT_SUCCESSFULLY_DELETED,
                        NotificationType = NotificationType.Success
                    };
                }
                else
                {
                    return new SingleResponse<ProductDTO>()
                    {
                        Success = true,
                        Message = ProductConstants.PRODUCT_CANT_BE_DELETED,
                        NotificationType = NotificationType.Info
                    };
                }
            }
            else
            {
                return new SingleResponse<ProductDTO>()
                {
                    Success = false,
                    Message = ProductConstants.PRODUCT_DOESNT_EXIST,
                    NotificationType = NotificationType.Info
                };
            }
        }
        catch (Exception ex)
        {
            return new SingleResponse<ProductDTO>()
            {
                Success = false,
                Message = ProductConstants.PRODUCT_DELETE_ERROR,
                ExceptionMessage = ex.Message,
                NotificationType = NotificationType.Error
            };
        }
    }

    public SingleResponse<ProductCreateDTO> UpdateProduct(Guid id, ProductCreateDTO request)
    {
        throw new NotImplementedException();
    }
}
