using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseItineraries
{
    [Table("AppCruiseItineraries")]
    [Audited]
    public class CruiseItinerary : Entity, IMayHaveTenant, IFullAudited
    {
        public int? TenantId { get; set; }

        //public virtual int SeasonYear { get; set; }

        public virtual string ItineraryName { get; set; }

        public virtual string ItineraryCode { get; set; }

        public virtual byte[] ItineraryMap { get; set; }

        public virtual string Description { get; set; }

        public virtual string Lang { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public virtual int? OnBoardService { get; set; }
    }
}
