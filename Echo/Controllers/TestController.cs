using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Echo.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        // GET api/test
        [HttpGet]
        public IActionResult Get()
        {
            return View();
        }

    }
}
