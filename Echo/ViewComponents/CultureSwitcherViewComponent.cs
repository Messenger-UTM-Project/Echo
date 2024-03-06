using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Echo.Models;

namespace Echo.ViewComponents
{
    public class CultureSwitcherViewComponent : ViewComponent
    {
		private readonly IOptions<RequestLocalizationOptions> localizationOptions;
        public CultureSwitcherViewComponent(IOptions<RequestLocalizationOptions> localizationOptions) =>
            this.localizationOptions = localizationOptions;
 
        public IViewComponentResult Invoke()
        {
            var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var model = new CultureSwitcher
            {
                SupportedCultures = [.. localizationOptions.Value.SupportedUICultures],
                CurrentUICulture = (cultureFeature != null) ? cultureFeature.RequestCulture.UICulture : null,
            };
            return View(model);
        }
    }
}

