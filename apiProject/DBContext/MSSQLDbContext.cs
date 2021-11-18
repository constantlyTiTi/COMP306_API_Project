using apiProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.DBContexts
{
    public class MSSQLDbContext:IdentityDbContext
    {
        public MSSQLDbContext(DbContextOptions<MSSQLDbContext> options) : base(options) { }
        public DbSet<Rate> Rate { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<OrderDetails> OrderDetail { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemFile> ItemFile { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if(modelBuilder.GetType() == typeof(User))
            {
                modelBuilder.Entity<User>()
                            .HasKey(i => i.UserName);
            }
            if (modelBuilder.GetType() == typeof(OrderDetails))
            {
                modelBuilder.Entity<OrderDetails>()
                            .HasKey(i => i.OrderId);
            }
            if (modelBuilder.GetType() == typeof(Item))
            {
                modelBuilder.Entity<Item>()
                            .HasKey(i => i.ItemId);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
