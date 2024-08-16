using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BurgerAssignmentFinal.Models;

namespace BurgerAssignmentFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsApiController : ControllerBase
    {
        private readonly BurgerManiaDBContext _context;

        public CartsApiController(BurgerManiaDBContext context)
        {
            _context = context;
        }

        // GET: api/CartsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts.ToListAsync();
        }

        // GET: api/CartsApi/id
        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(Guid userId)
        {
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Burger).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                return NotFound();
            }
            return cart;
        }

        // PUT: api/CartsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(Guid id, Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
               
        [HttpPut("{userId}/addToCart")]
        public async Task<ActionResult> PostCart(Guid userId, [FromBody] CartItem cartItem)
        {
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Burger).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.BurgerId == cartItem.BurgerId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                _context.CartItems.Add(cartItem);
                cart.Items.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok(cart);
        }
        
        [HttpPut("{userId}/removeFromCart/{itemId}")]
        public async Task<IActionResult> DeleteCart([FromRoute] Guid userId, [FromRoute] Guid itemId)
        {
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Burger).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                return NotFound($"Cart for user {userId} not found.");
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == itemId);
            if (cartItem == null)
            {
                return NotFound($"Cart item with ID {itemId} not found.");
            }

            cart.Items.Remove(cartItem);
            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            Console.WriteLine("Updated Cart: " + cart);
            return Ok(cart);
        }

        private bool CartExists(Guid id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
