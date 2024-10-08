using EntityModels.Models;
using Main.DTOs.Product;
using Main.DTOs.Responses;
using Main.DTOs.Subcategory;

namespace Main.Interfaces;

public interface ISubcategoryService
{
    ApiResponse<List<SubcategoryDTO>> GetSubcategories(int? skip, int? take);
}
