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

    [HttpPost("Create")]
    public IActionResult Insert(string name)
    {
        var response = _categoryService.CreateCategory(name);
        return Ok(response);
    }
}
