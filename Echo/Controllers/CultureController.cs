using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("/culture")]
public class CultureController : Controller
{
	[HttpGet]
	[AllowAnonymous]
	[Route("", Name="SetCulture")]
    public IActionResult SetCulture(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
				SameSite = SameSiteMode.None,
				Secure = true
            }
        );

        return LocalRedirect(returnUrl);
    }
}
