using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PajoPhone.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<FieldsKey> FieldsKeys { get; set; } = null!;
        public DbSet<FieldsValue> FieldsValues { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasQueryFilter(c => c.DeletedAt == null);
            modelBuilder.Entity<FieldsKey>()
                .HasQueryFilter(f => f.DeletedAt == null);
            modelBuilder.Entity<FieldsValue>()
                .HasQueryFilter(f => f.DeletedAt == null);
            modelBuilder.Entity<FieldsKey>()
                .HasIndex(x => x.CategoryId);
            modelBuilder.Entity<FieldsKey>()
                .HasOne(x => x.Category).WithMany(x => x.FieldsKeys).HasForeignKey(x => x.CategoryId).IsRequired(true);
        }
    }
}