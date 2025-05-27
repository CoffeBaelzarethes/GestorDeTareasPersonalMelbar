using System.ComponentModel.DataAnnotations;

namespace GestorDeTareasMelbar.Database.Tables
{
    public class Integrante
    {
        [Key]
        public int IdIntegrante { get; set; }
        [StringLength(65)]
        public string Nombre { get; set; }

        public ICollection<ProyectoIntegrante> ProyectoIntegrantes { get; set; } = [];
    }
}