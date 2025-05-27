using System.ComponentModel.DataAnnotations;

namespace GestorDeTareasMelbar.Database.Tables
{
    public class Tarea
    {
        [Key]
        public int IdTarea { get; set; }
        public string Nombre { get; set; }
        public byte Estado { get; set; }
        public DateTime? Vencimiento { get; set; }
        [StringLength(100)]
        public string? Nota { get; set; }

        public int Grupo_idGrupo { get; set; }
        public Grupo? Grupo { get; set; }
    }
}