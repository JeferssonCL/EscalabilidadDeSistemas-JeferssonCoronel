using Backend.Business.Services.Interfaces;
using Backend.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await service.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await service.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        var newProduct = await service.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (id != product.Id) return BadRequest();
        var updatedProduct = await service.UpdateProductAsync(product);
        return Ok(updatedProduct);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await service.DeleteProductAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
    
    [HttpDelete("/dispose/clear-cache")]
    public async Task<IActionResult> ClearCache()
    {
        var data = await service.ClearProductCache();
        return Ok(data);
    }
}