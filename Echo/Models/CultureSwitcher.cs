using System.Globalization;

namespace Echo.Models
{
    public class CultureSwitcher
    {
        public CultureInfo? CurrentUICulture { get; set; }
        public List<CultureInfo>? SupportedCultures { get; set; }
    }
}
