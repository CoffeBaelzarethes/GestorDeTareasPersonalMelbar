using GestorDeTareasMelbar.Database;
using GestorDeTareasMelbar.Database.Tables;
using GestorDeTareasMelbar.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GestorDeTareasMelbar.Controllers
{
    [Route("api/integrantes")]
    [ApiController]
    public class IntegranteController : ControllerBase
    {

        MelbarDB melbarDB;

        public IntegranteController(MelbarDB melbarDB)
        {
            this.melbarDB = melbarDB;
        }

        [HttpGet]
        public IEnumerable<Integrante> Get()
        {
            return melbarDB.Integrante.ToList();
        }

        [HttpGet("integrante/{id:int}")]
        public ActionResult<Integrante> Get(int id)
        {
            var integrante = melbarDB.Integrante.FirstOrDefault<Integrante>(i => i.IdIntegrante == id);

            if (integrante is null)
            {
                return NotFound();
            }

            return integrante;
        }

        [HttpGet("integrante/proyectos/{id:int}")]
        public ActionResult<IEnumerable<ProyectoIntegrante>> GetProyectos(int id)
        {
            List<ProyectoIntegrante> proyectoIntegrantes = melbarDB.ProyectoIntegrante
                .Where(pi => pi.IntegranteIdIntegrante == id).ToList();

            if (proyectoIntegrantes is null || proyectoIntegrantes.Count == 0)
            {
                return NotFound();
            }

            return proyectoIntegrantes;
        }

        [HttpPost]
        public ActionResult Post(IntegranteCreacionDTO integrante)
        {
            var entity = melbarDB.Integrante.Add(new Integrante
            {
                Nombre = integrante.Nombre
            });

            melbarDB.SaveChanges();

            // Agregando los datos a la tabla intermedia que vincula los proyectos con los integrantes
            foreach (int proyectoId in integrante.ProyectoIds)
            {
                if(melbarDB.Proyecto.Any(p => p.idProyecto == proyectoId))
                {
                    melbarDB.ProyectoIntegrante.Add(new ProyectoIntegrante
                    {
                        IntegranteIdIntegrante = entity.Entity.IdIntegrante,
                        ProyectoIdProyecto = proyectoId
                    });
                }
            }

            melbarDB.SaveChanges();

            Console.WriteLine(integrante.ProyectoIds);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, IntegranteCreacionDTO integrante)
        {
            if (!melbarDB.Integrante.Any<Integrante>(i => i.IdIntegrante == id))
            {
                return BadRequest();
            }

            melbarDB.Update(new Integrante
            {
                IdIntegrante = id,
                Nombre = integrante.Nombre,
            });
            melbarDB.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            int elementosBorrados = melbarDB.Integrante.Where<Integrante>(t => t.IdIntegrante== id)
                .ExecuteDelete();

            if (elementosBorrados == 0)
            {
                return NotFound();
            }

            melbarDB.SaveChanges();

            return NoContent();
        }
    }
}
