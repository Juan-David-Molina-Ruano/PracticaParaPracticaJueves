using Microsoft.EntityFrameworkCore;

namespace PracticaParaPracticaJueves.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Phone> Phones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(p => p.Phones)
                .WithOne(d => d.Customer)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
