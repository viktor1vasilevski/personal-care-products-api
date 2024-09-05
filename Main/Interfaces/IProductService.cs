using EntityModels.Models;
using static Main.Services.ProductService;

namespace Main.Interfaces;

public interface IProductService
{
    IEnumerable<ProductDto> GetProducts(string category, string subcategory, int skip, int take);
}
