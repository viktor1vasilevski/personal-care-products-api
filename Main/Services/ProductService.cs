using Data.Context;
using EntityModels.Interfaces;
using EntityModels.Models;
using Main.Interfaces;

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

    public IEnumerable<Product> GetProducts()
    {
        var products = _productRepository.Get();
        return products;
    }
}
