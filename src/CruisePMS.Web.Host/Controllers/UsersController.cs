using Abp.AspNetCore.Mvc.Authorization;
using CruisePMS.Authorization;
using CruisePMS.Storage;
using Abp.BackgroundJobs;

namespace CruisePMS.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}