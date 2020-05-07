using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using CruisePMS.Authorization.Users;
using CruisePMS.MultiTenancy;

namespace CruisePMS.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}