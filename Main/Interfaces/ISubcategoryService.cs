﻿using Main.DTOs.Category;
using Main.DTOs.Subcategory;
using Main.Requests;
using Main.Responses;

namespace Main.Interfaces;

public interface ISubcategoryService
{
    QueryResponse<List<SubcategoryDTO>> GetSubcategories(SubcategoryRequest request);
    SingleResponse<SubcategoryDTO> GetSubcategoryById(Guid id);
    SingleResponse<SubcategoryDTO> DeleteSubcategory(Guid id);
    SingleResponse<SubcategoryDTO> CreateSubcategory(CreateUpdateSubcategoryDTO request);
    SingleResponse<SubcategoryDTO> UpdateSubcategory(Guid id, CreateUpdateSubcategoryDTO request);
    QueryResponse<List<SubcategoryDropdownListDTO>> GetSubcategoriesDropdownList();
}
