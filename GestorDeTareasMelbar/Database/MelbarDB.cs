using GestorDeTareasMelbar.Controllers;
using GestorDeTareasMelbar.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace GestorDeTareasMelbar.Database
{
    public class MelbarDB : DbContext
    {
        public MelbarDB(DbContextOptions<MelbarDB> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Grupo>()
                .HasOne(g => g.Proyecto)
                .WithMany(p => p.Grupos)
                .HasForeignKey(g => g.Proyecto_idProyecto);
        }

        // Ejemplo: DbSet para una tabla
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<Integrante> Integrante { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<Grupo> Grupo { get; set; }
    }
}
