using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPortal.Data;
using MiPortal;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Cart/{userId} - Obtener el carrito de un usuario
    [HttpGet("{userId}")]
    public async Task<ActionResult<Cart>> GetCart(int userId)
    {
        var cart = await _context.Carts
                                 .Include(c => c.CartItems)
                                 .ThenInclude(ci => ci.Product)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return NotFound("Cart not found");

        return cart;
    }

    // POST: /Cart/Add - Añadir un ítem al carrito usando webid
    [HttpPost("Add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Webid == request.Webid);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var cart = await _context.Carts
                                 .Include(c => c.CartItems)
                                 .FirstOrDefaultAsync(c => c.UserId == user.UserId);
        if (cart == null)
        {
            cart = new Cart { UserId = user.UserId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var existingCartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == request.ProductId);
        if (existingCartItem == null)
        {
            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            _context.CartItems.Add(cartItem);
        }
        else
        {
            existingCartItem.Quantity += request.Quantity;
        }
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    public class AddToCartRequest
    {
        public string? Webid { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }


    // PUT: /Cart/{userId}/Update - Actualizar un ítem en el carrito
    [HttpPut("{userId}/Update")]
    public async Task<IActionResult> UpdateCartItem(int userId, [FromBody] CartItem cartItem)
    {
        var cartItemToUpdate = await _context.CartItems
                                     .FirstOrDefaultAsync(ci => ci.CartId == cartItem.CartId && ci.ProductId == cartItem.ProductId);
        if (cartItemToUpdate == null)
            return NotFound("Cart item not found");

        cartItemToUpdate.Quantity = cartItem.Quantity;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: /Cart/{userId}/Remove/{itemId} - Eliminar un ítem del carrito
    [HttpDelete("{userId}/Remove/{itemId}")]
    public async Task<IActionResult> RemoveFromCart(int userId, int itemId)
    {
        var cartItem = await _context.CartItems
                                     .FirstOrDefaultAsync(ci => ci.CartItemId == itemId);
        if (cartItem == null)
            return NotFound("Cart item not found");

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: /Cart/{userId}/Clear - Vaciar el carrito
    [HttpDelete("{userId}/Clear")]
    public async Task<IActionResult> ClearCart(int userId)
    {
        var cart = await _context.Carts
                                 .Include(c => c.CartItems)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null)
            return NotFound("Cart not found");

        if (cart.CartItems != null && cart.CartItems.Any())
        {
            _context.CartItems.RemoveRange(cart.CartItems);
        }
        
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
