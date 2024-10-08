using Main.Interfaces;
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


    [HttpGet("GetAll")]
    public IActionResult Get()
    {
        var response = _subcategoryService.GetSubcategories(0, 10);
        return Ok(response);
    }
}
