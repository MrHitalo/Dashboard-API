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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (pedido == null) return NotFound(new { message = "Pedido não encotrado"}); 

        return Ok(pedido);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PedidoCreateRequest request )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //Verifica se já existe um pedido com esse número
        if (await _context.Pedidos.AnyAsync(p => p.Numero == request.Numero)) 
            return BadRequest(new { message = "Já existe um pedido com este número" });

        // Criar o pedido
        var pedido = new Pedido { 
            Numero = request.Numero,
            NumeroCliente = request.NumeroCliente,
            Cliente= request.Cliente,
            Status = request.Status ?? "Pendente",
            Prioridade = request.Prioridade ?? "Normal",
            DataEntrada = request.DataEntrada ?? DateTime.UtcNow,
            CreatedBy = request.CreatedBy
        };

        if (request.Items != null && request.Items.Any())
        {
            foreach (var itemRequest in request.Items)
            {
                var item = new PedidoItem
                {
                    Item = itemRequest.Item,
                    Quantidade = itemRequest.Quantidade,
                    ValorUnitario = itemRequest.ValorUnitario,
                    Observacao = itemRequest.Observacao
                };
                pedido.Items.Add(item);
            }
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Recarrega o pedido com os items para retornar completo
        await _context.Entry(pedido).Collection(p => p.Items).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);



    }
}
