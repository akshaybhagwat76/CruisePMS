using Abp.Auditing;
using CruisePMS.Configuration.Dto;

namespace CruisePMS.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}