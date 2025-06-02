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
            var newTarea = melbarDB.Tarea.Add(new Tarea
            {
                Nombre = tarea.Nombre,
                Estado = tarea.Estado,
                Nota = tarea.Nota,
                Vencimiento = tarea.Vencimiento,
                Grupo_idGrupo = tarea.Grupo_idGrupo
            });
            melbarDB.SaveChanges();

            TareaToShowDTO toShow = new TareaToShowDTO()
            {
                IdTarea = newTarea.Entity.IdTarea,
                Nombre = newTarea.Entity.Nombre,
                Estado = newTarea.Entity.Estado,
                Vencimiento = newTarea.Entity.Vencimiento,
                Nota = newTarea.Entity.Nota,
                Grupo_idGrupo = newTarea.Entity.Grupo_idGrupo
            };

            return Ok(toShow);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, TareaCreacionDTO tarea)
        {
            var entidad = melbarDB.Tarea.FirstOrDefault(t => t.IdTarea == id);
            if (entidad == null)
                return NotFound();

            entidad.Nombre = tarea.Nombre;
            entidad.Estado = tarea.Estado;
            entidad.Nota = tarea.Nota;
            entidad.Vencimiento = tarea.Vencimiento;
            entidad.Grupo_idGrupo = tarea.Grupo_idGrupo;

            melbarDB.SaveChanges();

            TareaToShowDTO toShow = new TareaToShowDTO
            {
                IdTarea = entidad.IdTarea,
                Nombre = entidad.Nombre,
                Estado = entidad.Estado,
                Vencimiento = entidad.Vencimiento,
                Nota = entidad.Nota,
                Grupo_idGrupo = entidad.Grupo_idGrupo
            };

            return Ok(toShow);
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
