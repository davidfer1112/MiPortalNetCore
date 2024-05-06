using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPortal.Data;
using MiPortal;

[ApiController]
[Route("[controller]")]
public class OrderDetailsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrderDetailsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /OrderDetails
    [HttpGet]
    public async Task<IActionResult> GetOrderDetails()
    {
        var orderDetails = await _context.OrderDetails.Include(od => od.Order).Include(od => od.Product).ToListAsync();
        return Ok(orderDetails);
    }

    // GET: /OrderDetails/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetail(int id)
    {
        var orderDetail = await _context.OrderDetails.Include(od => od.Order).Include(od => od.Product)
                                                     .FirstOrDefaultAsync(od => od.OrderDetailId == id);
        if (orderDetail == null)
            return NotFound();

        return Ok(orderDetail);
    }

    // POST: /OrderDetails
    [HttpPost]
    public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetail orderDetail)
    {
        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.OrderDetailId }, orderDetail);
    }

    // PUT: /OrderDetails/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetail orderDetailDetails)
    {
        if (id != orderDetailDetails.OrderDetailId)
            return BadRequest("OrderDetail ID mismatch");

        _context.Entry(orderDetailDetails).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.OrderDetails.Any(e => e.OrderDetailId == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    // DELETE: /OrderDetails/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderDetail(int id)
    {
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail == null)
            return NotFound();

        _context.OrderDetails.Remove(orderDetail);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
