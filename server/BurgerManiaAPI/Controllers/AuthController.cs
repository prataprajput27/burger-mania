using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BurgerAssignmentFinal.Models;
using Microsoft.EntityFrameworkCore;
using BurgerManiaAPI.Services;

namespace BurgerAssignmentFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BurgerManiaDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public BurgerManiaDBContext Context { get; }
        public IConfiguration Object { get; }

        public AuthController(BurgerManiaDBContext context, IConfiguration configuration, ITokenService tokenService) 
        {
            _context = context;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return BadRequest("User already registered.");
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Phone = long.Parse(model.Phone),
                Password = model.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || user.Password != model.Password)
                return Unauthorized("Invalid credentials.");

            var token = _tokenService.GenerateToken(model.Email);

            return Ok(new
            {
                Token = token,
                UserId = user.Id.ToString(),
                User = user
            });
        }

    }
}