using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BurgerAssignmentFinal.Models;
using Microsoft.AspNetCore.Authorization;

namespace BurgerAssignmentFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BurgersApiController : ControllerBase
    {
        private readonly BurgerManiaDBContext _context;

        public BurgersApiController(BurgerManiaDBContext context)
        {
            _context = context;
        }

        // GET: api/BurgersApi
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Burger>>> GetBurgers()
        {
            return await _context.Burgers.ToListAsync();
        }

        // GET: api/BurgersApi/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Burger>> GetBurgers(Guid id)
        {
            var burgers = await _context.Burgers.FindAsync(id);

            if (burgers == null)
            {
                return NotFound();
            }

            return burgers;
        }

        // PUT: api/BurgersApi/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBurgers(Guid id, Burger burgers)
        {
            if (id != burgers.Id)
            {
                return BadRequest();
            }

            _context.Entry(burgers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BurgersExists(id))
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

        // POST: api/BurgersApi
        [HttpPost]
        public async Task<ActionResult<Burger>> PostBurgers(Burger burgers)
        {
            _context.Burgers.Add(burgers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBurgers", new { id = burgers.Id }, burgers);
        }

        // DELETE: api/BurgersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBurgers(Guid id)
        {
            var burgers = await _context.Burgers.FindAsync(id);
            if (burgers == null)
            {
                return NotFound();
            }

            _context.Burgers.Remove(burgers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BurgersExists(Guid id)
        {
            return _context.Burgers.Any(e => e.Id == id);
        }
    }
}
