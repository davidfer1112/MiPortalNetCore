using Microsoft.EntityFrameworkCore;

namespace TuNamespace.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Aquí puedes definir las propiedades DbSet para tus modelos
        // public DbSet<TuModelo> Modelos { get; set; }
    }
}