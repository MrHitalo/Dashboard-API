namespace Dashboard.Controllers;

using Dashboard.Data;
using Dashboard.DTOs.Pedidos;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller de pedidos - CRUD completo de pedidos
/// </summary>

[Route("api/pedidos")]
public class PedidosController: ControllerBase
{
    private readonly AppDbContext _context;

    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.Items) // join
            .OrderByDescending(p => p.CreatedAt) // orderBy
            .ToListAsync();

        return Ok(pedidos); 
    }

    public async Task<IActionResult> GetById(int id)
    {
        var pedidoId = await _context.Pedidos
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        return Ok(pedidoId);
    }
}
