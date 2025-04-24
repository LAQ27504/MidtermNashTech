using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Infrastructure.Persistence.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=LibraryDb;User Id=sa;Password=SQLServer1@;TrustServerCertificate=True;"
                );

                Console.WriteLine("Connection successfully");
            }
        }
    }
}
