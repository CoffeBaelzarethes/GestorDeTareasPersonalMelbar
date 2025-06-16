using GestorDeTareasMelbar.Database;
using GestorDeTareasMelbar.Database.Tables;
using GestorDeTareasMelbar.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                return Ok(null);
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

            foreach (ProyectoIntegrante pi in proyectoIntegrantes)
            {
                Trace.WriteLine("IdIntegrante: " + pi.IntegranteIdIntegrante + " IdProyecto: " + pi.ProyectoIdProyecto);
            }

            return Ok(proyectoIntegrantes);
        }


        [HttpGet("por-proyecto/{proyectoId:int}")]
        public ActionResult<IEnumerable<Integrante>> GetIntegrantesPorProyecto(int proyectoId)
        {
            var integrantes = melbarDB.ProyectoIntegrante
                .Where(pi => pi.ProyectoIdProyecto == proyectoId)
                .Include(pi => pi.Integrante)
                .Select(pi => pi.Integrante)
                .ToList();

            if (integrantes == null || integrantes.Count == 0)
                return NotFound("No hay integrantes en ese proyecto");

            return Ok(integrantes);
        }

        [HttpGet("existe/{nombre}")]
        public async Task<IActionResult> ExisteIntegrantePorNombre(string nombre)
        {
            var existe = await melbarDB.Integrante
                .AnyAsync(i => i.Nombre.ToLower() == nombre.ToLower());

            return Ok(existe); // true o false
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


        [HttpPost("integrante/proyecto/por_nombre")]
        public async Task<ActionResult> ByName(AgregarIntegranteDTO dto)
        {
            var integrante = melbarDB.Integrante.Where<Integrante>(i => i.Nombre == dto.Nombre).First();

            if (integrante == null)
            {
                return Ok("El integrante no existe");
            }

            var relacion = new ProyectoIntegrante
            {
                IntegranteIdIntegrante = integrante.IdIntegrante,
                ProyectoIdProyecto = dto.ProyectoIdProyecto
            };

            melbarDB.ProyectoIntegrante.Add(relacion);
            await melbarDB.SaveChangesAsync();

            return Ok();
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
                return Ok(new { mensaje = "No se encontró el registro" });
            }

            melbarDB.SaveChanges();

            return Ok();
        }

        [HttpDelete("by_name/{nombre}")]
        public ActionResult DeleteByName(string nombre)
        {
            int elementosBorrados = melbarDB.Integrante.Where<Integrante>(t => t.Nombre == nombre)
                .ExecuteDelete();

            if (elementosBorrados == 0)
            {
                return Ok(new { mensaje = "No se encontró el registro" });
            }

            melbarDB.SaveChanges();

            return Ok();
        }


        [HttpDelete("relacion/{idProyecto:int}/{nombre}")]
        public ActionResult EliminarRelacion(int idProyecto, string nombre)
        {
            var integrante = melbarDB.Integrante
                .FirstOrDefault(i => i.Nombre == nombre);

            if (integrante == null)
            {
                return Ok(new { mensaje = "El integrante no existe" });

            }

            var relacion = melbarDB.ProyectoIntegrante
                .FirstOrDefault(r => r.ProyectoIdProyecto == idProyecto && r.IntegranteIdIntegrante == integrante.IdIntegrante);

            if (relacion == null)
            {
                return Ok(new { mensaje = "La relación no existe: " + idProyecto + " Integrante: " + integrante.IdIntegrante });
            }

            melbarDB.ProyectoIntegrante.Remove(relacion);
            melbarDB.SaveChanges();

            return Ok(new { mensaje = "Relación eliminada" });

        }


    }
}

        
