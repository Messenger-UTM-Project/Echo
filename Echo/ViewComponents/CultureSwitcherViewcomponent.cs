using Echo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using System.Linq;
 
namespace Echo.ViewComponents
{
    public class CultureSwitcherViewComponent : ViewComponent
    {
        private readonly IOptions<RequestLocalizationOptions> localizationOptions;
		private readonly ILogger<CultureSwitcherViewComponent> _logger;

        public CultureSwitcherViewComponent(IOptions<RequestLocalizationOptions> localizationOptions, ILogger<CultureSwitcherViewComponent> logger)
		{
            localizationOptions = localizationOptions;
			_logger = logger;
		}
 
        public IViewComponentResult Invoke()
        {
			 _logger.LogInformation(string.Join(", ", localizationOptions.Value.SupportedCultures));
            var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var model = new CultureSwitcher
            {
                SupportedCultures = localizationOptions.Value.SupportedUICultures.ToList(),
                CurrentUICulture = cultureFeature.RequestCulture.UICulture
            };
            return View(model);
        }
    }
}

