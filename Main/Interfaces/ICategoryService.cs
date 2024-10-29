using Main.DTOs.Category;
using Main.Responses;

namespace Main.Interfaces;

public interface ICategoryService
{
    QueryResponse<List<CategoryDTO>> GetCategories(int? skip, int? take, string? sort, string? name);
    SingleResponse<CategoryDTO> CreateCategory(CreateUpdateCategoryDTO request);
    SingleResponse<CategoryDTO> UpdateCategory(Guid id, CreateUpdateCategoryDTO request);
    SingleResponse<CategoryDTO> DeleteCategory(Guid id);
    SingleResponse<CategoryDTO> GetCategoryById(Guid id);
}
