using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorDeTareasMelbar.Database.Tables
{
    public class Grupo
    {
        [Key]
        public int IdGrupo { get; set; }

        [Required] // Campo obligatorio
        [StringLength(45)] // Longitud máxima para VARCHAR
        public string Nombre { get; set; }

        public int Proyecto_IdProyecto { get; set; }

        public Proyecto Proyecto { get; set; }

        public ICollection<Tarea> Tareas { get; set; } = [];


    }
}