using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPortal.Data;
using MiPortal.Models;  // Asegúrate de incluir tus modelos, si están en un namespace separado
using MiPortal.Services;  // Asegúrate de que tu servicio de correo está correctamente referenciado

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public OrdersController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // GET: /Orders
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _context.Orders.Include(o => o.User).Include(o => o.OrderDetails).ToListAsync();
        return Ok(orders);
    }

    // GET: /Orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _context.Orders.Include(o => o.User).Include(o => o.OrderDetails)
                                         .FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    // POST: /Orders
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Preparar y enviar el correo electrónico
        var user = await _context.Users.FindAsync(order.UserId);
        if (user != null)
        {
            var email = new EmailDTO
            {
                Para = user.Email,
                Asunto = "Confirmación de Orden",
                Contenido = $"Hola {user.Username},<br/>Tu orden con ID {order.OrderId} ha sido creada exitosamente."
            };

            _emailService.SendEmail(email);
        }

        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
    }

    // PUT: /Orders/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order orderDetails)
    {
        if (id != orderDetails.OrderId)
            return BadRequest("Order ID mismatch");

        _context.Entry(orderDetails).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Orders.Any(e => e.OrderId == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    // DELETE: /Orders/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound();

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
