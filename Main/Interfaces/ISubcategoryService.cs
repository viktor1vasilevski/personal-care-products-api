using EntityModels.Models;
using Main.DTOs.Category;
using Main.DTOs.Product;
using Main.DTOs.Responses;
using Main.DTOs.Subcategory;
using Main.Responses;

namespace Main.Interfaces;

public interface ISubcategoryService
{
    ApiResponse<List<SubcategoryDTO>> GetSubcategories(int? skip, int? take);
    SingleResponse<SubcategoryDTO> GetByIdSubcategory(Guid id);
}
