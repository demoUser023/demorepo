using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Product
    {
        [Key]
        public int PId { get; set; }
        [Required]
        public string PName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required,Column(TypeName ="varchar(255)")]
        public string PImage { get; set; }
        [Required]
        public string Product_Desc { get; set; }

    }
}
