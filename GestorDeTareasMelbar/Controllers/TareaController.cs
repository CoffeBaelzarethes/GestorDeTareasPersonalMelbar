using GestorDeTareasMelbar.Database;
using GestorDeTareasMelbar.Database.Tables;
using GestorDeTareasMelbar.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorDeTareasMelbar.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    public class TareaController : ControllerBase
    {

        MelbarDB melbarDB;

        public TareaController(MelbarDB melbarDB)
        {
            this.melbarDB = melbarDB;
        }

        [HttpGet]
        public IEnumerable<Tarea> Get()
        {
            return melbarDB.Tarea.ToList();
        }

        [HttpGet("tarea/{id:int}")]
        public ActionResult<Tarea> Get([FromRoute] int id)
        {
            var tarea = melbarDB.Tarea.FirstOrDefault<Tarea>(i => i.IdTarea == id);

            if(tarea is null)
            {
                return NotFound();
            }

            return tarea;
        }

        [HttpPost]
        public ActionResult Post(TareaCreacionDTO tarea)
        {
            melbarDB.Tarea.Add(new Tarea
            {
                Nombre = tarea.Nombre,
                Estado = tarea.Estado,
                Nota = tarea.Nota,
                Vencimiento = tarea.Vencimiento,
                Grupo_idGrupo = tarea.Grupo_idGrupo
            });
            melbarDB.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, TareaCreacionDTO tarea)
        {
            if(!melbarDB.Tarea.Any<Tarea>(t => t.IdTarea == id))
            {
                return BadRequest();
            }

            melbarDB.Update(new Tarea { 
                IdTarea = id, Nombre = tarea.Nombre, Estado = tarea.Estado,
                Nota = tarea.Nota, Vencimiento = tarea.Vencimiento, 
                Grupo_idGrupo = tarea.Grupo_idGrupo
            });
            melbarDB.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            int elementosBorrados = melbarDB.Tarea.Where<Tarea>(t => t.IdTarea == id).ExecuteDelete();
            
            if(elementosBorrados == 0)
            {
                return NotFound();
            }

            melbarDB.SaveChanges();

            return NoContent();
        }
    }
}
