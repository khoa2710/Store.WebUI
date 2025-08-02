using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.WebUI.Entities;
using Store.WebUI.Entity;
namespace Store.WebUI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            { 
                entity.ToTable ("Categories");
                entity.HasKey (x => x.Id);
                entity.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();

                entity.Property(c => c.Name).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Description).HasMaxLength(500);
                entity.Property(c => c.ImageUrl).HasMaxLength(255);
                entity.Property(c => c.Icon).HasMaxLength(255);
                entity.Property(c => c.Slug).HasMaxLength(255);
            });
            //config entity Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).UseIdentityColumn().ValueGeneratedOnAdd();



                entity.Property(c => c.Name).IsRequired().HasMaxLength(255);
                //--------------
                entity.Property(c => c.Description).HasMaxLength(255);
                entity.Property(c => c.Price).HasColumnType("decimal(18,2)");
                entity.Property(c => c.ImageUrl).HasMaxLength(255);
                //--
                entity.Property(c => c.DiscountAmount).HasColumnType("decimal(18,2)");
                entity.Property(c => c.Badges);
                entity.Property(c => c.Star);
            });
            //config entity Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                entity.Property(c => c.Name).IsRequired().HasMaxLength(255);
                entity.Property(c => c.OrderDate).IsRequired();
                entity.Property(c => c.OrderAddress).IsRequired().HasMaxLength(255);
                entity.Property(c => c.BillingAddress).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Status).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Note).IsRequired().HasMaxLength(255);

            });
            //config entity Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                entity.Property(c => c.Name).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Address).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Phone).IsRequired().HasMaxLength(255);
            });
            //config enity OrderDetails
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("Order Details");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                entity.Property(c => c.Name).IsRequired().HasMaxLength(255);
                //-------------------------------
                entity.HasIndex(c => new { c.OrderId, c.ProductId }).IsUnique();// unique index
                entity.Property(c => c.Price).HasColumnType("decimal(18,2)");
            });
        }



    }
}
