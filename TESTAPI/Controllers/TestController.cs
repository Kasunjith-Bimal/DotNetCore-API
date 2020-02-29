using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Controllers
{
    public class TestController:Controller
    {
        [HttpGet("api/ok")]
        public IActionResult Get()
        {
            return Ok(new { name ="kasun"});
        }
    }
}
