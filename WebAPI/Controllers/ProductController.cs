using Main.DTOs.Category;
using Main.DTOs.Product;
using Main.Interfaces;
using Main.Requests;
using Main.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    // random comment
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

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        var response = _productService.GetProductById(id);
        return Ok(response);
    }

    [HttpPost("Create")]
    public IActionResult Insert([FromBody] ProductCreateDTO model)
    {
        //if (!ModelState.IsValid)
        //    return BadRequest(ModelState);

        var response = _productService.CreateProduct(model);
        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    public IActionResult Delete(Guid id)
    {
        var response = _productService.DeleteProduct(id);
        return Ok(response);
    }

    [HttpPut("Update/{id}")]
    public IActionResult Update(Guid id, [FromBody] ProductCreateDTO request)
    {
        var response = _productService.UpdateProduct(id, request);
        return Ok(response);
    }

}
