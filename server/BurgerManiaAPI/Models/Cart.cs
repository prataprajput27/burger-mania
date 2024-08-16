using System.ComponentModel.DataAnnotations.Schema;

namespace BurgerAssignmentFinal.Models
{
    [Table("Cart", Schema = "Cart")]
    public class Cart
    {
        public Cart()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<CartItem>? Items { get; set; } = new List<CartItem>();
    }

    [Table("CartItem", Schema = "CartItem")]
    public class CartItem
    {
        public CartItem()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid BurgerId { get; set; }
        public Burger? Burger { get; set; }
        public int Quantity { get; set; }
    }
}
