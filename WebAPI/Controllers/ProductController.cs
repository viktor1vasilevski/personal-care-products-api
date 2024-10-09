using Main.DTOs.Product;
using Main.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }


    [HttpGet("Get")]
    public IActionResult Get(string? category, string? subCategory, int? skip, int? take)
    {
        var response = _productService.GetProducts(category, subCategory, skip, take);
        return Ok(response);
    }

    [HttpPost("Create")]
    public IActionResult Insert(ProductCreateDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = _productService.CreateProduct(model);
        return Ok(response);
    }

}
