using EntityModels.Models;
using Main.DTOs.Category;
using Main.DTOs.Product;
using Main.DTOs.Responses;

namespace Main.Interfaces;

public interface ICategoryService
{
    ApiResponse<List<CategoryDTO>> GetCategories(int? skip, int? take);
}
