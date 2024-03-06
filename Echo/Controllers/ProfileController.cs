using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Echo.Data;

namespace Echo.Controllers
{
    [Route("/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<ProfileController> _localizer;

        public ProfileController(AppDbContext context, IStringLocalizer<ProfileController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("", Name = "Profile")]
        public IActionResult Index()
        {
            // Logic for the profile index action
            return Ok("Profile Index");
        }

        [HttpGet]
        [Route("{guid:guid}", Name = "GUIDProfile")]
        public IActionResult GetProfileByGuid(Guid guid)
        {
            // Logic to retrieve profile by GUID
            return Ok($"Retrieving profile by GUID: {guid}");
        }

        [HttpGet]
        [Route("{username}", Name = "UsernameProfile")]
        public IActionResult GetProfileByUsername(string username)
        {
            // Logic to retrieve profile by either username or GUID
            return Ok($"Retrieving profile by username: {username}");
        }
    }
}
