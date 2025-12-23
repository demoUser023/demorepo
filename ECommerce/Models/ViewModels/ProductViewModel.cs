using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace ECommerce.Models.ViewModels;
public class ProductViewModel
{
    public int PId { get; set; }

    [Required]
    public string PName { get; set; }

    [Required]
    public decimal Price { get; set; }

    public IFormFile PImageFile { get; set; }

    public string PImage { get; set; } 

    public string Product_Desc { get; set; }
}
