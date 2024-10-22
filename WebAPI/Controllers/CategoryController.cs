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
    public IActionResult Get(int? skip, int? take)
    {
        var response = _categoryService.GetCategories(skip, take);
        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        var response = _categoryService.GetByIdCategory(id);
        return Ok(response);
    }

    [HttpPost("Create")]
    public IActionResult Insert([FromBody] CreateCategoryDTO request)
    {
        var response = _categoryService.CreateCategory(request);
        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    public IActionResult Delete(Guid id)
    {
        var response = _categoryService.DeleteCategory(id);
        return Ok(response);
    }
}
