using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Context
{
    public class ECommerceDBContext :DbContext
    {
        public ECommerceDBContext(DbContextOptions<ECommerceDBContext> options) : base(options)
        {

            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
