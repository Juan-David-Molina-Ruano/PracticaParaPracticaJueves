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
        public DbSet<Rol> Rols { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasMany(p => p.Phones)
                .WithOne(d => d.Customer)
                .OnDelete(DeleteBehavior.Cascade);

            //insertar datos inicales a la tabla rols
            modelBuilder.Entity<Rol>().HasData(new Rol { Id = 1, Name = "Admin", Description = "soy admin" });

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "Root", RolId = 1, Password = "e10adc3949ba59abbe56e057f20f883e" });
        }
    }
}
