using Main.Interfaces;
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
    public IActionResult Get(int? skip, int? take)
    {
        var response = _subcategoryService.GetSubcategories(skip, take);
        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        var response = _subcategoryService.GetByIdSubcategory(id);
        return Ok(response);
    }
}
