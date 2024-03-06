using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;

using Echo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Echo.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IStringLocalizer<NavbarViewComponent> _localizer;
		private readonly IUrlHelperFactory _urlHelperFactory;

		public NavbarViewComponent(IStringLocalizer<NavbarViewComponent> localizer, IUrlHelperFactory urlHelperFactory)
        {
            _localizer = localizer;
			_urlHelperFactory = urlHelperFactory;
		}

        public IViewComponentResult Invoke(List<NavbarLink> links, ViewContext viewContext)
        {
			var _urlHelper = _urlHelperFactory.GetUrlHelper(viewContext);
			var model = links.Select(link =>
            {
                if (string.IsNullOrEmpty(link.Text))
                    link.Text = link.RouteName;

                link.RouteName = _urlHelper.RouteUrl(link.RouteName);
                return link;
            }).ToList();

            return View(model);
        }
    }
}
