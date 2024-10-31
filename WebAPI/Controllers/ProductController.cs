using Main.DTOs.Product;
using Main.Interfaces;
using Main.Requests;
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
    public IActionResult Get([FromQuery] ProductRequest request)
    {
        var response = _productService.GetProducts(request);
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
