using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace GestorDeTareasMelbar.Database.Tables
{
    public class ProyectoIntegrante
    {
        public int IntegranteIdIntegrante { get; set; }
        public Integrante Integrante { get; set; }

        public int ProyectoIdProyecto { get; set; }
        public Proyecto Proyecto { get; set; }
    }
}