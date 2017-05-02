using System.Collections.Generic;
using System.Web.Mvc;

namespace AspNet.Identity.MySQL.WebApiModels
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
    }
}