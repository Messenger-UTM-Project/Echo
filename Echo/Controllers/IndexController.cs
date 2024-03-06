using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using Echo.Data;

namespace Echo.Controllers
{
    [Route("")]
    public class IndexController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<IndexController> _localizer;

        public IndexController(AppDbContext context, IStringLocalizer<IndexController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("", Name = "Index")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("403", Name = "403")]
        public IActionResult Page403()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("404", Name = "404")]
        public IActionResult Page404()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("error", Name = "Error")]
        public IActionResult ErrorPage()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("login", Name = "Login")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpGet]
        [Route("logout", Name = "Logout")]
        public IActionResult Logout()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signin", Name = "SignIn")]
        public string SignIn()
        {
            return "Ok";
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup", Name = "SignUp")]
        public string SignUp()
        {
            return "Ok";
        }
    }
}
