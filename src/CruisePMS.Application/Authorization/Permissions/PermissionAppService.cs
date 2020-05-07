using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using CruisePMS.Authorization.Permissions.Dto;
using CruisePMS.Authorization.Roles;
using CruisePMS.MultiTenancy.Payments;

namespace CruisePMS.Authorization.Permissions
{
    public class PermissionAppService : CruisePMSAppServiceBase, IPermissionAppService
    {
        private readonly RoleManager _roleManager;

        public PermissionAppService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions()
        {
            IReadOnlyList<Permission> permissions;

            if (AbpSession.MultiTenancySide != Abp.MultiTenancy.MultiTenancySides.Host)
            {
                var res = (GetCurrentTenant()).TenantType;
                permissions = PermissionManager.GetAllPermissions();
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    Role d;
                    Permission[] grantedPermissions;
                    switch (res)
                    {
                        case TenantType.Cruise:
                            d = _roleManager.GetRoleByName(StaticRoleNames.Host.Cruise);
                            grantedPermissions = (_roleManager.GetGrantedPermissionsAsync(d).GetAwaiter().GetResult()).ToArray();
                            permissions = permissions.Where(x => grantedPermissions.Any(z => z.Name == x.Name)).Select(x => x).ToList(); break;
                        case TenantType.Travel:
                            d = _roleManager.GetRoleByName(StaticRoleNames.Host.Cruise);
                            grantedPermissions = (_roleManager.GetGrantedPermissionsAsync(d).GetAwaiter().GetResult()).ToArray();
                            permissions = permissions.Where(x => grantedPermissions.Any(z => z.Name == x.Name)).Select(x => x).ToList();
                            break;
                        case TenantType.Ship:
                            d = _roleManager.GetRoleByName(StaticRoleNames.Host.Cruise);
                            grantedPermissions = (_roleManager.GetGrantedPermissionsAsync(d).GetAwaiter().GetResult()).ToArray();
                            permissions = permissions.Where(x => grantedPermissions.Any(z => z.Name == x.Name)).Select(x => x).ToList();
                            break;
                    }
                }
            }
            else
            {
                permissions = PermissionManager.GetAllPermissions();
            }

            var rootPermissions = permissions.Where(p => p.Parent == null);

            var result = new List<FlatPermissionWithLevelDto>();

            foreach (var rootPermission in rootPermissions)
            {
                var level = 0;
                AddPermission(rootPermission, permissions, result, level);
            }

            return new ListResultDto<FlatPermissionWithLevelDto>
            {
                Items = result
            };
        }

        private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<FlatPermissionWithLevelDto> result, int level)
        {
            var flatPermission = ObjectMapper.Map<FlatPermissionWithLevelDto>(permission);
            flatPermission.Level = level;
            result.Add(flatPermission);

            if (permission.Children == null)
            {
                return;
            }

            var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

            foreach (var childPermission in children)
            {
                AddPermission(childPermission, allPermissions, result, level + 1);
            }
        }
    }
}