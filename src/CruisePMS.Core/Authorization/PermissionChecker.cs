using Abp.Authorization;
using CruisePMS.Authorization.Roles;
using CruisePMS.Authorization.Users;

namespace CruisePMS.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
