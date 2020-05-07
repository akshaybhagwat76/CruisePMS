using System.Collections.Generic;
using CruisePMS.Authorization.Permissions.Dto;

namespace CruisePMS.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}