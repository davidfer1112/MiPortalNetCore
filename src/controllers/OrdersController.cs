using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPortal.Data;
using MiPortal.Models;
using MiPortal.Services;
using System.IO;
using System.Threading.Tasks;

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
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDTO orderRequest)
    {
        var order = new Order
        {
            UserId = orderRequest.UserId,
            OrderDate = orderRequest.OrderDate.ToUniversalTime(), // Convertir a UTC
            Status = orderRequest.Status,
            TotalAmount = orderRequest.TotalAmount
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Leer el contenido de la plantilla HTML
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "src", "template", "OrderConfirmationTemplate.html");
        if (!System.IO.File.Exists(templatePath))
        {
            return StatusCode(500, "Template file not found");
        }

        string emailContent = await System.IO.File.ReadAllTextAsync(templatePath);

        // Reemplazar los placeholders con los valores reales
        emailContent = emailContent.Replace("{USERNAME}", orderRequest.Username)
                                   .Replace("{ORDERID}", order.OrderId.ToString());

        // Preparar y enviar el correo electrónico
        var email = new EmailDTO
        {
            Para = orderRequest.Email,
            Asunto = "Confirmación de Orden",
            Contenido = emailContent
        };

        _emailService.SendEmail(email);

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

public class OrderRequestDTO
{
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
}
