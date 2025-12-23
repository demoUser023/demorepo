using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        public int Product_Id { get; set; }

        // Navigation property
        [ForeignKey("Product_Id")]
        public Product Product { get; set; }

    }
}
