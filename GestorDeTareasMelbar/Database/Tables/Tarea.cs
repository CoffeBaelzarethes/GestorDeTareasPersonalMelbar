using System.ComponentModel.DataAnnotations;

namespace GestorDeTareasMelbar.Database.Tables
{
    public class Tarea
    {
        public Guid idTarea { get; set; }
        [Required]
        [StringLength(45)]
        public string nombre { get; set; }
        public byte estado { get; set; }
        public DateTime? vencimiento { get; set; }
        [StringLength(100)]
        public string nota { get; set; }

        public Guid Grupo_idGrupo;
    }
}
