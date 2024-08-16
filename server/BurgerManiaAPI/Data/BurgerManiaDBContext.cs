using BurgerAssignmentFinal.Models;
using Microsoft.EntityFrameworkCore;

public class BurgerManiaDBContext : DbContext
{
    public BurgerManiaDBContext(DbContextOptions<BurgerManiaDBContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Burger> Burgers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
}
