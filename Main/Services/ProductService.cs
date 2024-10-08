using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.DTOs;
using Main.DTOs.Product;
using Main.DTOs.Responses;
using Main.Extensions;
using Main.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Main.Services;

public class ProductService : IProductService
{
    private IUnitOfWork<LibraryDbContext> _uow;
    private IGenericRepository<Product> _productRepository;

    public ProductService(IUnitOfWork<LibraryDbContext> uow)
    {
        _uow = uow;
        _productRepository = _uow.GetGenericRepository<Product>();
    }

    public ApiResponse<List<ProductDTO>> GetProducts(string? category, string? subCategory, int? skip, int? take)
    {
        try
        {
            var products = _productRepository.GetAsQueryable(null, null,
                        x => x.Include(x => x.Subcategory).ThenInclude(sc => sc.Category));

            products = products
                .WhereIf(!String.IsNullOrEmpty(category), x => x.Subcategory.Category.Name.ToLower() == category.ToLower())
                .WhereIf(!String.IsNullOrEmpty(subCategory), x => x.Subcategory.Name.ToLower() == subCategory.ToLower());

            if (skip.HasValue)
                products = products.Skip(skip.Value);

            if (take.HasValue)
                products = products.Take(take.Value);

            var productsDTO = products.Select(x => new ProductDTO
            {
                Id = x.Id,
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

            return new ApiResponse<List<ProductDTO>>() { Data = productsDTO, Success = true };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<ProductDTO>>() { Success = false, Message = "Se desi zbunka", ExceptionMessage = ex.Message };
        }
        
    }



}
