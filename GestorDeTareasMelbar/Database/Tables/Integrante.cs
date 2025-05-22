using System.ComponentModel.DataAnnotations;

namespace GestorDeTareasMelbar.Database.Tables
{
    public class Integrante
    {
        public int idIntegrante { get; set; }
        [StringLength(60)]
        public string nombre { get; set; }
    }
}
