using Abp.AspNetCore.Mvc.Authorization;
using CruisePMS.Storage;

namespace CruisePMS.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}