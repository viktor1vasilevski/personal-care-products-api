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

    public ProductService(IUnitOfWork<LibraryDbContext> uow)
    {
        _uow = uow;
        _productRepository = _uow.GetGenericRepository<Product>();
        _subcategoryRepository = _uow.GetGenericRepository<Subcategory>();
    }

    public QueryResponse<List<ProductDTO>> GetProducts(ProductRequest request)
    {
        try
        {
            var products = _productRepository.GetAsQueryableWhereIf(x => 
            x.WhereIf(!String.IsNullOrEmpty(request.Category), x => x.Subcategory.Category.Name.ToLower() == request.Category.ToLower())
             .WhereIf(!String.IsNullOrEmpty(request.SubCategory), x => x.Subcategory.Name.ToLower() == request.SubCategory.ToLower()), 
            null,
            x => x.Include(x => x.Subcategory).ThenInclude(sc => sc.Category));

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
                Edition = x.Edition,
                Category = x.Subcategory.Category.Name,
                Subcategory = x.Subcategory.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy
            }).ToList();

            return new QueryResponse<List<ProductDTO>>() 
            { 
                Success = true, 
                Data = productsDTO, 
                TotalCount = totalCount
            };
        }
        catch (Exception ex)
        {
            return new QueryResponse<List<ProductDTO>>() 
            { 
                Success = false, 
                Message = ProductConstants.ERROR_RETRIEVING_PRODUCTS, 
                ExceptionMessage = ex.Message 
            };
        }
        
    }

    public QueryResponse<ProductCreateDTO> CreateProduct(ProductCreateDTO model)
    {
        try
        {
            var subcategory = _subcategoryRepository.GetByID(model.SubcategoryId);
            if (subcategory is null)
                return new QueryResponse<ProductCreateDTO> { Success = false, Message = SubcategoryConstants.SUBCATEGORY_DOESNT_EXIST };

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
                //ImageData = model.ImageData,
                SubcategoryId = model.SubcategoryId,

            };

            _productRepository.Insert(entity);
            _uow.SaveChanges();

            return new QueryResponse<ProductCreateDTO>
            {
                Success = true,
                Data = model,
                Message = ProductConstants.PRODUCT_SUCCESSFULLY_CREATED
            };
        }
        catch (Exception ex)
        {
            return new QueryResponse<ProductCreateDTO>()
            {
                Success = false,
                Message = ProductConstants.PRODUCT_ERROR_CREATING,
                ExceptionMessage = ex.Message
            };
        }
    }

    public SingleResponse<ProductDTO> GetProductById(Guid id)
    {
        try
        {
            if (_productRepository.Exists(x => x.Id == id))
            {
                var product = _productRepository.GetAsQueryable(x => x.Id == id).Include(x => x.Subcategory).FirstOrDefault();

                return new SingleResponse<ProductDTO>()
                {
                    Success = true,
                    Message = ProductConstants.PRODUCT_SUCCESSFULLY_RETRIVED,
                    NotificationType = NotificationType.Success,
                    Data = new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Created = product.Created,
                        CreatedBy = product.CreatedBy,
                        LastModified = product.LastModified,
                        LastModifiedBy = product.LastModifiedBy,
                        //Subcategories = category.Subcategory?.Select(sc => sc.Name).ToList() ?? new List<string>()
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
}
