using Main.DTOs.Category;
using Main.DTOs.Product;
using Main.Requests;
using Main.Responses;

namespace Main.Interfaces;

public interface IProductService
{
    QueryResponse<List<ProductDTO>> GetProducts(ProductRequest request);
    SingleResponse<ProductDTO> GetProductById(Guid id);
    QueryResponse<ProductCreateDTO> CreateProduct(ProductCreateDTO model);
    SingleResponse<ProductDTO> DeleteProduct(Guid id);
}
