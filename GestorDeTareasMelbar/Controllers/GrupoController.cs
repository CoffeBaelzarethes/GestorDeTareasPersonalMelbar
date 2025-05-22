using GestorDeTareasMelbar.Database;
using GestorDeTareasMelbar.Database.Tables;
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
        public Grupo Get()
        {
            return melbarDB.Grupo.First<Grupo>(); ;
        }
    }
}
