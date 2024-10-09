using Main.DTOs;
using Main.DTOs.Product;
using Main.DTOs.Responses;

namespace Main.Interfaces;

public interface IProductService
{
    ApiResponse<List<ProductDTO>> GetProducts(string? category, string? subcategory, int? skip, int? take);
    ApiResponse<ProductCreateDTO> CreateProduct(ProductCreateDTO model);
}
