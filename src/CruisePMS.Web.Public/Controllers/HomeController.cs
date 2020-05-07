using Microsoft.AspNetCore.Mvc;
using CruisePMS.Web.Controllers;

namespace CruisePMS.Web.Public.Controllers
{
    public class HomeController : CruisePMSControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}