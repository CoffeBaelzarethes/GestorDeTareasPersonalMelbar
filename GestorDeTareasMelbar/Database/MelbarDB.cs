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
                .HasForeignKey(g => g.Proyecto_IdProyecto);

            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Grupo)
                .WithMany(g => g.Tareas)
                .HasForeignKey(g => g.Grupo_idGrupo);

            modelBuilder.Entity<ProyectoIntegrante>()
                .HasKey(pi => new { pi.ProyectoIdProyecto, pi.IntegranteIdIntegrante });

            modelBuilder.Entity<ProyectoIntegrante>()
                .HasOne(pi => pi.Proyecto)
                .WithMany(p => p.ProyectoIntegrantes) // ← asegúrate que esta colección exista
                .HasForeignKey(pi => pi.ProyectoIdProyecto);

            modelBuilder.Entity<ProyectoIntegrante>()
                .HasOne(pi => pi.Integrante)
                .WithMany(i => i.ProyectoIntegrantes) // ← también debe estar en `Integrante`
                .HasForeignKey(pi => pi.IntegranteIdIntegrante);
        }

        // Ejemplo: DbSet para una tabla
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<Integrante> Integrante { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<Grupo> Grupo { get; set; }
        public DbSet<ProyectoIntegrante> ProyectoIntegrante { get; set; }
    }
}