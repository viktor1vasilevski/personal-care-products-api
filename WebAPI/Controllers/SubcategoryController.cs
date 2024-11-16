﻿using Main.DTOs.Category;
using Main.DTOs.Subcategory;
using Main.Interfaces;
using Main.Requests;
using Main.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubcategoryController : ControllerBase
{
    private readonly ISubcategoryService _subcategoryService;
    public SubcategoryController(ISubcategoryService subcategoryService)
    {
        _subcategoryService = subcategoryService;
    }


    [HttpGet("Get")]
    public IActionResult Get([FromQuery] SubcategoryRequest request)
    {
        var response = _subcategoryService.GetSubcategories(request);
        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        var response = _subcategoryService.GetSubcategoryById(id);
        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    public IActionResult Delete(Guid id)
    {
        var response = _subcategoryService.DeleteSubcategory(id);
        return Ok(response);
    }

    [HttpPost("Create")]
    public IActionResult Insert([FromBody] CreateUpdateSubcategoryDTO request)
    {
        var response = _subcategoryService.CreateSubcategory(request);
        return Ok(response);
    }

    [HttpPut("Update/{id}")]
    public IActionResult Update(Guid id, [FromBody] CreateUpdateSubcategoryDTO request)
    {
        var response = _subcategoryService.UpdateSubcategory(id, request);
        return Ok(response);
    }
}
