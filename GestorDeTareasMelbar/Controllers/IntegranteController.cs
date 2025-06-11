using GestorDeTareasMelbar.Database;
using GestorDeTareasMelbar.Database.Tables;
using GestorDeTareasMelbar.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

            return Ok(integrante);
        }

        [HttpGet("integrante/proyectos/{id:int}")]
        public ActionResult<IEnumerable<ProyectoIntegrante>> GetProyectos(int id)
        {
            List<ProyectoIntegrante> proyectoIntegrantes = melbarDB.ProyectoIntegrante
                .Where(pi => pi.IntegranteIdIntegrante == id).ToList();

            if (proyectoIntegrantes is null || proyectoIntegrantes.Count == 0)
            {
                return NotFound("No se han encontrado proyectos asociados a ese id");
            }

            foreach(ProyectoIntegrante pi in proyectoIntegrantes)
            {
                Trace.WriteLine("IdIntegrante: " + pi.IntegranteIdIntegrante + " IdProyecto: " + pi.ProyectoIdProyecto);
            }

            return Ok(proyectoIntegrantes);
        }

        [HttpPost]
        public ActionResult<Integrante> Post(IntegranteCreacionDTO integrante)
        {
            var entity = melbarDB.Integrante.Add(new Integrante
            {
                Nombre = integrante.Nombre
            });

            melbarDB.SaveChanges();

            return Ok(entity.Entity);
        }

        [HttpPost("integrante/proyecto")]
        public ActionResult<Integrante> PostIntegranteProyecto(IntegranteProyectoCreacionDTO dto)
        {
            var entity = melbarDB.ProyectoIntegrante.Add(new ProyectoIntegrante()
            {
                IntegranteIdIntegrante = dto.IntegranteIdIntegrante,
                ProyectoIdProyecto = dto.ProyectoIdProyecto
            });

            melbarDB.SaveChanges();

            return Ok(entity.Entity);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Integrante> Put(int id, IntegranteCreacionDTO integrante)
        {
            if (!melbarDB.Integrante.Any<Integrante>(i => i.IdIntegrante == id))
            {
                return BadRequest();
            }

            var entity = melbarDB.Update(new Integrante
            {
                IdIntegrante = id,
                Nombre = integrante.Nombre,
            });
            melbarDB.SaveChanges();

            return Ok(entity.Entity);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            int elementosBorrados = melbarDB.Integrante.Where<Integrante>(t => t.IdIntegrante == id)
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
