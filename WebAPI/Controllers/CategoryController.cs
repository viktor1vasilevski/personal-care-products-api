using Main.DTOs.Category;
using Main.Interfaces;
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
    public IActionResult Get(int? skip, int? take, string? sort, string? name)
    {
        var response = _categoryService.GetCategories(skip, take, sort, name);
        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        var response = _categoryService.GetCategoryById(id);
        return Ok(response);
    }

    [HttpPost("Create")]
    public IActionResult Insert([FromBody] CreateUpdateCategoryDTO request)
    {
        var response = _categoryService.CreateCategory(request);
        return Ok(response);
    }

    [HttpPut("Update/{id}")]
    public IActionResult Update(Guid id, [FromBody] CreateUpdateCategoryDTO request)
    {
        var response = _categoryService.UpdateCategory(id, request);
        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    public IActionResult Delete(Guid id)
    {
        var response = _categoryService.DeleteCategory(id);
        return Ok(response);
    }
}
