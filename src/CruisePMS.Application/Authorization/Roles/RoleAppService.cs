using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Zero.Configuration;
using Microsoft.EntityFrameworkCore;
using CruisePMS.Authorization.Permissions;
using CruisePMS.Authorization.Permissions.Dto;
using CruisePMS.Authorization.Roles.Dto;
using Abp.Domain.Uow;
using CruisePMS.MultiTenancy.Payments;

namespace CruisePMS.Authorization.Roles
{
    /// <summary>
    /// Application service that is used by 'role management' page.
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Administration_Roles)]
    public class RoleAppService : CruisePMSAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IRoleManagementConfig _roleManagementConfig;

        public RoleAppService(
            RoleManager roleManager,
            IRoleManagementConfig roleManagementConfig)
        {
            _roleManager = roleManager;
            _roleManagementConfig = roleManagementConfig;
        }

        public async Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input)
        {
            var query = _roleManager.Roles;

            if (input.Permissions != null && input.Permissions.Any(p => !string.IsNullOrEmpty(p)))
            {
                input.Permissions = input.Permissions.Where(p => !string.IsNullOrEmpty(p)).ToList();

                var staticRoleNames = _roleManagementConfig.StaticRoles.Where(
                    r => r.GrantAllPermissionsByDefault &&
                         r.Side == AbpSession.MultiTenancySide
                ).Select(r => r.RoleName).ToList();

                foreach (var permission in input.Permissions)
                {
                    query = query.Where(r =>
                        r.Permissions.Any(rp => rp.Name == permission)
                            ? r.Permissions.Any(rp => rp.Name == permission && rp.IsGranted)
                            : staticRoleNames.Contains(r.Name)
                    );
                }
            }

            var roles = await query.ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            var query = _roleManager.Roles;

            IReadOnlyList<Permission> permissions;


            if (AbpSession.MultiTenancySide != Abp.MultiTenancy.MultiTenancySides.Host)
            {
                var res = (GetCurrentTenant()).TenantType;
                permissions = PermissionManager.GetAllPermissions();
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    Role d;
                    Permission[] grantedPermissionsts;
                    switch (res)
                    {
                        case TenantType.Cruise:
                            d = _roleManager.GetRoleByName(StaticRoleNames.Host.Cruise);
                            grantedPermissionsts = (_roleManager.GetGrantedPermissionsAsync(d).GetAwaiter().GetResult()).ToArray();
                            permissions = permissions.Where(x => grantedPermissionsts.Any(z => z.Name == x.Name)).Select(x => x).ToList(); break;
                        case TenantType.Travel:
                            d = _roleManager.GetRoleByName(StaticRoleNames.Host.Cruise);
                            grantedPermissionsts = (_roleManager.GetGrantedPermissionsAsync(d).GetAwaiter().GetResult()).ToArray();
                            permissions = permissions.Where(x => grantedPermissionsts.Any(z => z.Name == x.Name)).Select(x => x).ToList();
                            break;
                        case TenantType.Ship:
                            d = _roleManager.GetRoleByName(StaticRoleNames.Host.Cruise);
                            grantedPermissionsts = (_roleManager.GetGrantedPermissionsAsync(d).GetAwaiter().GetResult()).ToArray();
                            permissions = permissions.Where(x => grantedPermissionsts.Any(z => z.Name == x.Name)).Select(x => x).ToList();
                            break;
                    }
                }
            }
            else
            {
                if (query.Where(x => x.Name != StaticRoleNames.Host.Admin).Any(x => x.Id == input.Id))
                {
                    permissions = PermissionManager.GetAllPermissions().Where(x => x.MultiTenancySides != Abp.MultiTenancy.MultiTenancySides.Host).ToList();
                }
                else
                {
                    permissions = PermissionManager.GetAllPermissions();
                }
            }

         

            var grantedPermissions = new Permission[0];
            RoleEditDto roleEditDto;

            if (input.Id.HasValue) //Editing existing role?
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = ObjectMapper.Map<RoleEditDto>(role);
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        public async Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            if (input.Role.Id.HasValue)
            {
                await UpdateRoleAsync(input);
            }
            else
            {
                await CreateRoleAsync(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Delete)]
        public async Task DeleteRole(EntityDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            var users = await UserManager.GetUsersInRoleAsync(role.Name);
            foreach (var user in users)
            {
                CheckErrors(await UserManager.RemoveFromRoleAsync(user, role.Name));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Edit)]
        protected virtual async Task UpdateRoleAsync(CreateOrUpdateRoleInput input)
        {
            Debug.Assert(input.Role.Id != null, "input.Role.Id should be set.");
            var role = await _roleManager.GetRoleByIdAsync(input.Role.Id.Value);
            role.DisplayName = input.Role.DisplayName;
            role.IsDefault = input.Role.IsDefault;
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
            await UpdateAllTenantPermission(input);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create)]
        protected virtual async Task CreateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = new Role(AbpSession.TenantId, input.Role.DisplayName) { IsDefault = input.Role.IsDefault };
            CheckErrors(await _roleManager.CreateAsync(role));
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
            await UpdateAllTenantPermission(input);
        }

        [UnitOfWork]
        private async Task UpdateAllTenantPermission(CreateOrUpdateRoleInput input)
        {
            List<int> tenantId = new List<int>();

            if (input.Role.DisplayName == StaticRoleNames.Host.Cruise)
            {
                tenantId = TenantManager.Tenants.IgnoreQueryFilters().Where(x => x.TenantType == MultiTenancy.Payments.TenantType.Cruise).Select(x => x.Id).ToList();
            }
            else if (input.Role.DisplayName == StaticRoleNames.Host.Cruise)
            {
                tenantId = TenantManager.Tenants.IgnoreQueryFilters().Where(x => x.TenantType == MultiTenancy.Payments.TenantType.Ship).Select(x => x.Id).ToList();
            }
            else if (input.Role.DisplayName == StaticRoleNames.Host.Cruise)
            {
                tenantId = TenantManager.Tenants.IgnoreQueryFilters().Where(x => x.TenantType == MultiTenancy.Payments.TenantType.Travel).Select(x => x.Id).ToList();
            }
            foreach (var item in tenantId)
            {
                using (UnitOfWorkManager.Current.SetTenantId(item))
                {
                    var rolesAdmin = _roleManager.Roles.ToList();

                    foreach (var item2 in rolesAdmin)
                    {
                        var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(item2)).Where(x => x.MultiTenancySides != Abp.MultiTenancy.MultiTenancySides.Host).ToArray();

                        var d = grantedPermissions.Where(x => input.GrantedPermissionNames.Any(z => z == x.Name)).Select(x => x.Name).ToList();

                        await UpdateGrantedPermissionsAsync(item2, d);
                    }
                }
            }
            CurrentUnitOfWork.SaveChanges();
        }

        private async Task UpdateGrantedPermissionsAsync(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(grantedPermissionNames);
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
    }
}
