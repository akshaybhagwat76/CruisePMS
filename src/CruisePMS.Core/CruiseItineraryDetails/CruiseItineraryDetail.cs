using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.CruiseItineraries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseItineraryDetails
{
    [Table("AppCruiseItineraryDetail")]
    [Audited]
    public class CruiseItineraryDetail : Entity, IMayHaveTenant, IFullAudited
    {
        public int? TenantId { get; set; }


        public virtual int Day { get; set; }

        public virtual string PortID { get; set; }

        public virtual bool Breakfast { get; set; }

        public virtual bool Lunch { get; set; }

        public virtual bool AfternoonSnack { get; set; }

        public virtual bool Dinner { get; set; }

        public virtual bool CaptainDinner { get; set; }

        public virtual bool LiveMusic { get; set; }

        public virtual bool OnAnchor { get; set; }
        public virtual string Note { get; set; }

        public virtual string Description { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual int? CruiseItinerariesId { get; set; }

        [ForeignKey("CruiseItinerariesId")]
        public CruiseItinerary CruiseItinerariesFk { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
