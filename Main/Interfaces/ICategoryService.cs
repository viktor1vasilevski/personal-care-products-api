using EntityModels.Models;
using Main.DTOs.Category;
using Main.DTOs.Product;
using Main.DTOs.Responses;
using Main.Responses;

namespace Main.Interfaces;

public interface ICategoryService
{
    ApiResponse<List<CategoryDTO>> GetCategories(int? skip, int? take);
    SingleResponse<CategoryDTO> CreateCategory(CreateUpdateCategoryDTO request);
    SingleResponse<CategoryDTO> UpdateCategory(Guid id, CreateUpdateCategoryDTO request);
    SingleResponse<CategoryDTO> DeleteCategory(Guid id);
    SingleResponse<CategoryDTO> GetCategoryById(Guid id);
}
