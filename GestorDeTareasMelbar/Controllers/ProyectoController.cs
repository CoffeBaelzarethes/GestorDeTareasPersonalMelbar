using GestorDeTareasMelbar.Database;
using GestorDeTareasMelbar.Database.Tables;
using GestorDeTareasMelbar.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            return Ok(proyectoExistente);
        }



        [HttpDelete("{id}")]
        public ActionResult<Proyecto> DeleteProyecto(int id)
        {
            var proyecto = melbarDB.Proyecto.FirstOrDefault(p => p.idProyecto == id);

            if (proyecto == null)
            {
                //Trace.WriteLine("Not Found");
                return NotFound();
            }

            /*var grupos = melbarDB.Grupo
                .Where(g => g.Proyecto_IdProyecto == proyecto.idProyecto)
                .ToList(); // Fuerzo la carga completa antes del foreach

            foreach (Grupo grupo in grupos) // grupos en vez de esto porque no se pueden usar las
                                            // conecciones simultaneamente melbarDB.Grupo.Where(g =>
                                            // g.Proyecto_IdProyecto == proyecto.idProyecto))
            {
                melbarDB.Tarea.Where(t => t.Grupo_idGrupo == grupo.IdGrupo).ExecuteDelete();
                melbarDB.SaveChanges();
            }*/

            using var tx = melbarDB.Database.BeginTransaction();

            var grupoIds = melbarDB.Grupo
                .Where(g => g.Proyecto_IdProyecto == proyecto.idProyecto)
                .Select(g => g.IdGrupo)
                .ToList();

            melbarDB.Tarea
                .Where(t => grupoIds.Contains(t.Grupo_idGrupo))
                .ExecuteDelete();

            melbarDB.Grupo.Where(g => g.Proyecto_IdProyecto == proyecto.idProyecto).ExecuteDelete();

            melbarDB.ProyectoIntegrante.Where(pi => pi.ProyectoIdProyecto == proyecto.idProyecto).ExecuteDelete();

            melbarDB.Proyecto.Remove(proyecto);

            tx.Commit();

            return NoContent();
        }
    }
}
