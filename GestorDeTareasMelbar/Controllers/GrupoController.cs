using GestorDeTareasMelbar.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public ActionResult Get()
        {
            return melbarDB.Grupo;
        }
    }
}
