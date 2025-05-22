using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorDeTareasMelbar.Database.Tables
{
    public class Grupo
    {
        [Key]
        public Guid idGrupo { get; set; }

        [Required] // Campo obligatorio
        [StringLength(45)] // Longitud máxima para VARCHAR
        public string nombre { get; set; }

        [ForeignKey("Proyecto")]
        public int Proyecto_idProyecto { get; set; }

        public Proyecto Proyecto { get; set; }

        public ICollection<Tarea> Tareas { get; set; }


    }
}
