using EntityModels.Models;
using Main.DTOs.Category;
using Main.DTOs.Product;
using Main.DTOs.Responses;
using Main.DTOs.Subcategory;
using Main.Responses;

namespace Main.Interfaces;

public interface ISubcategoryService
{
    QueryResponse<List<SubcategoryDTO>> GetSubcategories(int? skip, int? take);
    SingleResponse<SubcategoryDTO> GetByIdSubcategory(Guid id);
}
