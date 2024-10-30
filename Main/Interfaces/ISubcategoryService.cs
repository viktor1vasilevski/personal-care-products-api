using Main.DTOs.Category;
using Main.DTOs.Subcategory;
using Main.Responses;

namespace Main.Interfaces;

public interface ISubcategoryService
{
    QueryResponse<List<SubcategoryDTO>> GetSubcategories(int? skip, int? take);
    SingleResponse<SubcategoryDTO> GetSubcategoryById(Guid id);
    SingleResponse<SubcategoryDTO> DeleteSubcategory(Guid id);
}
