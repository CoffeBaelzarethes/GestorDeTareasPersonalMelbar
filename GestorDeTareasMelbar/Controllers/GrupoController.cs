using GestorDeTareasMelbar.Database;
using GestorDeTareasMelbar.Database.Tables;
using GestorDeTareasMelbar.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorDeTareasMelbar.Controllers
{
    [Route("api/grupo")]
    [ApiController]
    public class GrupoController : ControllerBase
    {

        MelbarDB melbarDB;

        public GrupoController(MelbarDB melbarDB)
        {
            this.melbarDB = melbarDB;
        }


        [HttpGet("{id}")]
        public ActionResult<Grupo> GetGrupo(int id)
        {
            var grupo = melbarDB.Grupo.FirstOrDefault(g => g.IdGrupo == id);

            if (grupo == null)
            {
                return NotFound("Grupo no encontrado.");
            }

            return Ok(grupo);
        }


        [HttpGet]
        public ActionResult<IEnumerable<Grupo>> GetGrupos()
        {

            var grupos = melbarDB.Grupo.ToList();

            if (grupos == null || grupos.Count == 0)  // ver si no hay registros en la bd
            {
                return NotFound("No hay grupos encontrados");
            }

            return Ok(grupos);

        }


        [HttpPost]
        public ActionResult<Grupo> PostGrupo([FromBody] GrupoCreacionDTO dto)
        {
            var nuevoGrupo = new Grupo
            {
                Nombre = dto.Nombre,
                Proyecto_IdProyecto = dto.Proyecto_idProyecto
            };

            if (!melbarDB.Proyecto.Any(p => p.idProyecto == dto.Proyecto_idProyecto))
            {
                return BadRequest("El proyecto asociado no existe.");
            }
            
            melbarDB.Grupo.Add(nuevoGrupo);
            melbarDB.SaveChanges();

            return CreatedAtAction(nameof(GetGrupo), new { id = nuevoGrupo.IdGrupo }, nuevoGrupo);
        }


        [HttpPut("{id}")]
        public ActionResult PutGrupo(int id, GrupoCreacionDTO dto)
        {
            var grupoExistente = melbarDB.Grupo.FirstOrDefault(g => g.IdGrupo == id);
            if (grupoExistente == null)
                return NotFound("No se encontró el grupo");

            if(!melbarDB.Proyecto.Any(p => p.idProyecto == dto.Proyecto_idProyecto)) // Any: Determina si un elemento de una secuencia existe o satisface una condición.

            Console.WriteLine("Updating group");

            grupoExistente.Nombre = dto.Nombre;
            grupoExistente.Proyecto_IdProyecto = dto.Proyecto_idProyecto;

            melbarDB.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult<Grupo> DeleteGrupo(int id)
        {
            var grupo = melbarDB.Grupo.FirstOrDefault(g => g.IdGrupo == id);

            if (grupo == null)
            {
                return NotFound();
            }

            var books = melbarDB.Tarea.Where(i => i.Grupo_idGrupo == id);
            foreach(var book in books)
            {
                melbarDB.Tarea.Remove(book);
            }

            melbarDB.Grupo.Remove(grupo);
            melbarDB.SaveChanges();

            return NoContent();
        }

    }
}
