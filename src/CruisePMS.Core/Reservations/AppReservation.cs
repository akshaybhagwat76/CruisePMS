using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.Reservations
{
    [Table("Reservations")]
    [Audited]
    public class Reservation : Entity<long>, IMayHaveTenant, IFullAudited
    {

        public virtual int? TenantId { get; set; }

        [ForeignKey("ClientId")]
        public Client ClientFk { get; set; }

        public virtual long? ClientId { get; set; }

        public virtual int? ReservationNo { get; set; }

        public virtual long? TenantIdSupplier { get; set; }

        public virtual int? Season { get; set; }

        public virtual string ActionSource { get; set; }

        public virtual long? ReservationStatus { get; set; }

        public virtual long? CruiseId { get; set; }

        public virtual long? ShipId { get; set; }

        public virtual long? DepartureId { get; set; }

        public virtual DateTime? DepartureDate { get; set; }

        //public virtual decimal? OperatorDiscount { get; set; }

        //public virtual decimal? FareDiscount { get; set; }

        //public virtual decimal? Reduction { get; set; }

        public virtual decimal? ReservationTotal { get; set; }

        public virtual decimal? DepositAmount { get; set; }

        public virtual DateTime? DepositDate { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual decimal? AtEmbarcation { get; set; }

        public virtual string CurrencyID { get; set; }

        public virtual string CurrencyID2 { get; set; }

        public virtual string Note { get; set; }

        public virtual string Lang { get; set; }

        public virtual DateTime? ValidUpTo { get; set; }

        public virtual bool? ReservationSaved { get; set; }

        public virtual bool? ReservationLocked { get; set; }

        public virtual long? CreatorUserId { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual long? LastModifierUserId { get; set; }

        public virtual DateTime? LastModificationTime { get; set; }

        public virtual long? DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        public virtual bool IsDeleted { get; set; }

    }
}
