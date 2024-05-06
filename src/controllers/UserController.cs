using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPortal.Data;
using MiPortal;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Users
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    // GET: /Users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }

// POST: /Users
[HttpPost]
public async Task<IActionResult> CreateUser([FromBody] User user)
{
    _context.Users.Add(user);
    await _context.SaveChangesAsync(); // Asegúrate de que el usuario se guarda y obtiene un ID antes de crear el carrito.

    var cart = new Cart
    {
        UserId = user.UserId  // Asigna el UserId al carrito.
    };
    _context.Carts.Add(cart);
    await _context.SaveChangesAsync(); // Guarda el carrito en la base de datos.

    return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
}

    // PUT: /Users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User userDetails)
    {
        if (id != userDetails.UserId)
            return BadRequest();

        _context.Entry(userDetails).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(e => e.UserId == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: /Users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
