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


    [HttpGet]
    public IActionResult Get()
    {
        int skip = 0;
        int take = 10;
        var p = _productService.GetProducts("Soaps", "Beard", skip, take);
        return Ok(p);
    }

}
