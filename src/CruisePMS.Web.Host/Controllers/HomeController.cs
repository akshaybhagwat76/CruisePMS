using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace CruisePMS.Web.Controllers
{
    public class HomeController : CruisePMSControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
