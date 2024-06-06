using EntityModels.Models;

namespace Main.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetProducts();
}
