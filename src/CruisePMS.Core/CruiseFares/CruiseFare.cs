using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.Cruises;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseFares
{
    [Table("AppCruiseFare")]
    [Audited]
    public class CruiseFare: Entity, IMayHaveTenant, IFullAudited
    {
        public int? TenantId { get; set; }
        public virtual bool IsMainFare { get; set; }

        public virtual DateTime? FareStartDate { get; set; }

        public virtual DateTime? FareEndDate { get; set; }

        public virtual decimal Discount { get; set; }

        public virtual short DiscountType { get; set; }

        public virtual int ActivateDaysBeforeDeparture { get; set; }

        public virtual int? ShipId { get; set; }

        public virtual int? CruisesId { get; set; }

        [ForeignKey("CruisesId")]
        public Cruise CruisesFk { get; set; }

        public virtual int? FareName { get; set; }

        [ForeignKey("FareName")]
        public MasterAmenities FareNaFk { get; set; }

        public virtual bool FullPaymentRequired { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public long? DepartureId { get; set; }
        public DateTime? DepartureDate { get; set; }
    }
}
