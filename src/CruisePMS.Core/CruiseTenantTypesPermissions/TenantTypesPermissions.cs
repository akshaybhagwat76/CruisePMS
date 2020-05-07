using Abp.Auditing;
using Abp.Domain.Entities.Auditing;
using CruisePMS.CruiseTenantTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.CruiseTenantTypesPermissions
{
    [Table("AppTenantTypesPermissions")]
    [Audited]
    public class TenantTypesPermissions: AuditedEntity
    {
        public virtual string TenantRecipient { get; set; }
        [Required]
        public virtual string EntityName { get; set; }

        public virtual int TenantTypeID { get; set; }

        [ForeignKey("TenantTypeID")]
        public TenantTypes TenantTypesFk { get; set; }
    }
}
