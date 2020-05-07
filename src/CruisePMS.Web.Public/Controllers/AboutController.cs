using Microsoft.AspNetCore.Mvc;
using CruisePMS.Web.Controllers;

namespace CruisePMS.Web.Public.Controllers
{
    public class AboutController : CruisePMSControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}