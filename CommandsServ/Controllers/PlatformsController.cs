using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandsServ.Controllers{

    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
            
        }

        [HttpPost]
        public ActionResult TestInConnection()
        {
            Console.WriteLine("--> In Post # Command Service");
        
            return Ok("In Test of from Platform Service");
        }
    }
}