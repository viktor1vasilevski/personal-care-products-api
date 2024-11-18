using Main.DTOs.Category;
using Main.Requests;
using Main.Responses;

namespace Main.Interfaces;

public interface ICategoryService
{
    QueryResponse<List<CategoryDTO>> GetCategories(CategoryRequest request);
    SingleResponse<CategoryDTO> CreateCategory(CreateUpdateCategoryDTO request);
    SingleResponse<CategoryDTO> UpdateCategory(Guid id, CreateUpdateCategoryDTO request);
    SingleResponse<CategoryDTO> DeleteCategory(Guid id);
    SingleResponse<CategoryDTO> GetCategoryById(Guid id);
    QueryResponse<List<CategoryDropdownListDTO>> GetCategoriesDropdownList();
}
