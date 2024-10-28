﻿using Main.DTOs;
using Main.DTOs.Product;
using Main.DTOs.Responses;
using Main.Responses;

namespace Main.Interfaces;

public interface IProductService
{
    QueryResponse<List<ProductDTO>> GetProducts(string? category, string? subcategory, int? skip, int? take);
    QueryResponse<ProductCreateDTO> CreateProduct(ProductCreateDTO model);
}
