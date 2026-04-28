using Dashboard.Data;
using Dashboard.DTOs.Products;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers;

/// <summary>
/// Controller de produtos - CRUD completo de produtos
/// </summary>
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();
         return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound(new { message = "Produto não encontrado" });

        return Ok(product);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            Image = request.Image,
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id },product);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateRequest request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound(new { message = "Produto não encontrado" });

        if(!string.IsNullOrWhiteSpace(request.Name))
            product.Name = request.Name;

        if (request.Price.HasValue)
            product.Price = request.Price.Value; 

        if(request.Description != null)
            product.Description = request.Description;

        if(request.Image != null)
            product.Image = request.Image;

        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(product);
            
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound(new { message = "Produto não encontrado" });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Produto deletado com sucesso" });
    }

}
