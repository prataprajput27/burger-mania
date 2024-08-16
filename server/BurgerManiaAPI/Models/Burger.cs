using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BurgerAssignmentFinal.Models
{
    [Table("Burgers", Schema = "Burger")]
    public class Burger
    {
        public Burger()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string? B_Name { get; set; }
        [Required]
        public string? B_Image { get; set; }

        public string? B_Description { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
