using EntityModels.Models;

namespace Main.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetProducts(string category, string subcategory, int skip, int take);
}
