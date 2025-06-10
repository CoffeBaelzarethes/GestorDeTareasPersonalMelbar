using System.ComponentModel.DataAnnotations;

namespace GestorDeTareasMelbar.DTO
{
    public class IntegranteCreacionDTO {
        [StringLength(65)]
        public string Nombre { get; set; }
    }
}