using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace TrabajoPractico.Models
{
    public class appDBcontext : DbContext
    {
        public appDBcontext(DbContextOptions<appDBcontext> options)
         : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "Filename=trabajoPractico.db",
                sqliteOptionsAction: op =>
                {
                    op.MigrationsAssembly(
                        Assembly.GetExecutingAssembly().FullName
                        );
                });
        }
        public DbSet<Autor> autores { set; get; }
        public DbSet<Libro> libros { set; get; }
        public DbSet<Genero> generos { set; get; }

    }
}
