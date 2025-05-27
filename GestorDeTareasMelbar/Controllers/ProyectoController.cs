using GestorDeTareasMelbar.Database;
using GestorDeTareasMelbar.Database.Tables;
using GestorDeTareasMelbar.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeTareasMelbar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {

        MelbarDB melbarDB;

        public ProyectoController(MelbarDB melbarDB)
        {
            this.melbarDB = melbarDB;
        }

        [HttpGet("{id}")]
        public ActionResult<Proyecto> GetProyecto(int id)
        {
            var proyecto = melbarDB.Proyecto.FirstOrDefault(g => g.idProyecto == id);

            if (proyecto == null)
            {
                return NotFound("Proyecto no encontrado.");
            }

            return Ok(proyecto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Proyecto>> GetProyectos(int id)
        {
            var proyectos = melbarDB.Proyecto.ToList();

            if (proyectos == null || proyectos.Count == 0)  // ver si no hay registros en la bd
            {
                return NotFound("No hay grupos encontrados");
            }

            return Ok(proyectos);
        }


        [HttpPost]
        public ActionResult<Proyecto> PostProyecto([FromBody] ProyectoCreacionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return BadRequest("Es obligatorio que el proyecto tenga nombre");
            }

            var nuevoProyecto = new Proyecto
            {
                Nombre = dto.Nombre,
                Fecha_creacion = DateTime.Now
            };

            melbarDB.Proyecto.Add(nuevoProyecto);
            melbarDB.SaveChanges();

            return CreatedAtAction(nameof(GetProyecto), new { id = nuevoProyecto.idProyecto }, nuevoProyecto);
        }




        [HttpPut("{id}")]
        public ActionResult PutProyecto(int id, ProyectoCreacionDTO dto)
        {
            var proyectoExistente = melbarDB.Proyecto.FirstOrDefault(p => p.idProyecto == id);
            if (proyectoExistente == null)
                return NotFound("No se encontró el proyecto");

            proyectoExistente.Nombre = dto.Nombre;

            melbarDB.SaveChanges();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public ActionResult<Grupo> DeleteProyecto(int id)
        {
            var proyecto = melbarDB.Proyecto.FirstOrDefault(p => p.idProyecto == id);

            if (proyecto == null)
            {
                return NotFound();
            }

            melbarDB.Proyecto.Remove(proyecto);
            melbarDB.SaveChanges();

            return NoContent();
        }
    }
}
