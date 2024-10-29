using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Constants;
using Main.DTOs.Product;
using Main.Extensions;
using Main.Interfaces;
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

    public QueryResponse<List<ProductDTO>> GetProducts(string? category, string? subCategory, int? skip, int? take)
    {
        try
        {
            var products = _productRepository.GetAsQueryable(null, null,
                        x => x.Include(x => x.Subcategory).ThenInclude(sc => sc.Category));

            products = products
                .WhereIf(!String.IsNullOrEmpty(category), x => x.Subcategory.Category.Name.ToLower() == category.ToLower())
                .WhereIf(!String.IsNullOrEmpty(subCategory), x => x.Subcategory.Name.ToLower() == subCategory.ToLower());

            var totalCount = products.Count();

            if (skip.HasValue)
                products = products.Skip(skip.Value);

            if (take.HasValue)
                products = products.Take(take.Value);

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
                return new QueryResponse<ProductCreateDTO> { Success = false, Message = SubcategoryConstants.SUBCATEGORY_NOT_EXIST };

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
                Message = ProductConstants.SUCCESSFULLY_CREATED_PRODUCT
            };
        }
        catch (Exception ex)
        {
            return new QueryResponse<ProductCreateDTO>()
            {
                Success = false,
                Message = ProductConstants.ERROR_CREATING_PRODUCT,
                ExceptionMessage = ex.Message
            };
        }
    }
}
