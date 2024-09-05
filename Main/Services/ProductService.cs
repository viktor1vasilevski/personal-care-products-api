using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
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

    public IEnumerable<ProductDto> GetProducts(string category, string subCategory, int skip, int take)
    {
        var products = _productRepository.GetAsQueryable(
            x => x.Subcategory.Category.Name.ToLower() == category.ToLower() &&
                 x.Subcategory.Name.ToLower() == subCategory.ToLower(),
            null,
            x => x.Include(x => x.Subcategory)
                  .ThenInclude(sc => sc.Category)
        );



        products = products.Skip(skip).Take(take);

        var productDtos = products.Select(x => new ProductDto
        {
            Id = x.Id,
            Brand = x.Brand,
            Description = x.Description,
            UnitPrice = x.UnitPrice,
            UnitQuantity = x.UnitQuantity,
            Volume = x.Volume,
            Scent = x.Scent,
            Edition = x.Edition,
            SubcategoryName = x.Subcategory.Name,
            CategoryName = x.Subcategory.Category.Name
        }).ToList();

        return productDtos;
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitQuantity { get; set; }
        public int? Volume { get; set; }
        public string? Scent { get; set; }
        public string? Edition { get; set; }
        public string SubcategoryName { get; set; }
        public string CategoryName { get; set; }
    }

}
