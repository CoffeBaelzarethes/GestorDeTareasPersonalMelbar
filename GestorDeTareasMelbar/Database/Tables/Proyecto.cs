using System.ComponentModel.DataAnnotations;

namespace GestorDeTareasMelbar.Database.Tables
{
    public class Proyecto
    {
        [Key]
        public int idProyecto {  get; set; }

        [Required] // Campo obligatorio
        [StringLength(45)] // Longitud máxima para VARCHAR
        public string nombre { get; set; }

        public DateTime fecha_creacion { get; set; }

        public ICollection<Grupo> Grupos { get; set; } = [];
        public ICollection<ProyectoIntegrante> ProyectoIntegrantes { get; set; } = [];

    }
}
