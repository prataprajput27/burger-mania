using System.Globalization;

namespace BurgerManiaAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(string email);
    }
}
