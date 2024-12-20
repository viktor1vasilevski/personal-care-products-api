﻿using Main.DTOs.Category;
using Main.Interfaces;
using Main.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }


    [HttpGet("Get")]
    public IActionResult Get([FromQuery] CategoryRequest request)
    {
        var response = _categoryService.GetCategories(request);
        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        var response = _categoryService.GetCategoryById(id);
        return Ok(response);
    }

    [HttpPost("Create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public IActionResult Insert([FromBody] CreateUpdateCategoryDTO request)
    {
        var response = _categoryService.CreateCategory(request);
        return Ok(response);
    }

    [HttpPut("Update/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public IActionResult Update(Guid id, [FromBody] CreateUpdateCategoryDTO request)
    {
        var response = _categoryService.UpdateCategory(id, request);
        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public IActionResult Delete(Guid id)
    {
        var response = _categoryService.DeleteCategory(id);
        return Ok(response);
    }

    [HttpGet("GetCategoriesDropdown")]
    public IActionResult GetCategoriesDropdown()
    {
        var response = _categoryService.GetCategoriesDropdownList();
        return Ok(response);
    }

}
