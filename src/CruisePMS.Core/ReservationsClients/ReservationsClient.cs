using Abp.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.ReservationsClients
{
    [Table("AppReservationsClient")]
    [Audited]
    public class ReservationsClient : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        public virtual long? ReservationId { get; set; }

        public virtual string CabinIdentificator { get; set; }

        public virtual long? ClientId { get; set; }

        public virtual bool ReservationHolder { get; set; }

        public virtual int? AgePolicyId { get; set; }

        public virtual string Note { get; set; }
    }
}
