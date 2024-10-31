using Main.DTOs.Product;
using Main.Requests;
using Main.Responses;

namespace Main.Interfaces;

public interface IProductService
{
    QueryResponse<List<ProductDTO>> GetProducts(ProductRequest request);
    QueryResponse<ProductCreateDTO> CreateProduct(ProductCreateDTO model);
}
