using GestorDeTareasMelbar.Controllers;
using GestorDeTareasMelbar.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace GestorDeTareasMelbar.Database
{
    public class MelbarDB : DbContext
    {
        public MelbarDB(DbContextOptions<MelbarDB> options) : base(options) { }

        // Ejemplo: DbSet para una tabla
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<Integrante> Integrante { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<Grupo> Grupo { get; set; }
    }
}
