using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExchangeRates.ViewComponents
{
    public class CulturePicker : ViewComponent
    {
        private readonly RequestLocalizationOptions _localization;

        public CulturePicker(IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _localization = localizationOptions.Value;
        }

        public IViewComponentResult Invoke()
        {
            var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var model = new CulturePickerModel
            {
                SupportedCultures = _localization.SupportedUICultures.ToList(),
                CurrentUICulture = cultureFeature.RequestCulture.UICulture
            };

            return View(model);
        }
    }

    public class CulturePickerModel
    {
        public CultureInfo CurrentUICulture { get; set; }
        public List<CultureInfo> SupportedCultures { get; set; }

        public string ToFlagEmoji(string country)
        {
            country = country
                .Split('-')
                .LastOrDefault();

            if (country == null)
                return "⁉️️";

            return string.Concat(
                country
                .ToUpper()
                .Select(x => char.ConvertFromUtf32(x + 0x1F1A5))
            );
        }
    }
}