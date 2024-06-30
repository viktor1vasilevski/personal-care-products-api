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

    public IEnumerable<Product> GetProducts(string category, string subCategory, int skip, int take)
    {
        var products = _productRepository.GetAsQueryable(x => 
            x.Subcategory.Name.ToLower() == subCategory.ToLower() &&
            x.Subcategory.Category.Name == category, 
            null, 
            x => x.Include(x => x.Subcategory).ThenInclude(x => x.Category));


        products = products.Skip(skip).Take(take);


        return products;
    }
}
