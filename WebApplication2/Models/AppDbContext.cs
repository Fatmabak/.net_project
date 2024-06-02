using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
namespace WebApplication2.Models

{
    public class ProductDbContext : IdentityDbContext
    {
        public
        ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

    public DbSet<Product> Products { get; set; }



    }
}
