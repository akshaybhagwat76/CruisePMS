using Abp.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.BookingStatuses
{
    [Table("AppBookingStatus")]
    [Audited]
    public class BookingStatus : Entity<int>
    {
        public virtual string StatusColor { get; set; }

        public virtual int? Sort { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual int? TenantId { get; set; }

        public virtual decimal? Duration { get; set; }

        public virtual string Expr1 { get; set; }

    }
}
