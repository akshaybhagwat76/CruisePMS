using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.Cruises;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CharterDepartures
{
    [Table("AppCharterDeparture")]
    [Audited]
    public class CharterDeparture : Entity, IMayHaveTenant, IFullAudited
    {
        public int? TenantId { get; set; }
        public virtual int DepartureYear { get; set; }
        public virtual string SeasonGroup { get; set; }
        public virtual DateTime DepartureDate { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }


        public virtual int? CruiseShipId { get; set; }

        [ForeignKey("CruiseShipId")]
        public Cruise CruiseShipFk { get; set; }

    }
}
