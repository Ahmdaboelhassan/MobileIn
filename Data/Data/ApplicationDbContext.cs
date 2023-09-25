using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MobileIn.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
         public DbSet<Company> companies { get; set; }
         public DbSet<Processor> processors { get; set; }
         public DbSet<Mobile> mobiles { get; set; }
         public DbSet<ShoppingList> shoppingList { get; set; }
         public DbSet<OrderHeader> orderHeader { get; set; }
         public DbSet<OrderDatials> orderDatials { get; set; }
         public DbSet<ApplicationUser> applicationUser { get; set; }
         
    }
}