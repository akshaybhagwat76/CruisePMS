using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Authorization.Permissions.Dto;

namespace CruisePMS.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
