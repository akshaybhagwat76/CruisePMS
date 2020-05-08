using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.Common
{
    [Table("ReadSettings")]
    public class ReadSettings : Entity<long>
    {

        public virtual int TenantId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual string TenancyName { get; set; }

    }
}
