using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.CruiseServiceGroups;
using CruisePMS.CruiseServiceUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseServices
{
    [Table("AppCruiseService")]
    [Audited]
    public class CruiseService : Entity, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public virtual int? CruiseServiceGroupsId { get; set; }
        [ForeignKey("CruiseServiceGroupsId")]
        public CruiseServiceGroup CruiseServiceGroupsFk { get; set; }
        public virtual int? CruiseServiceUnitsId { get; set; }
        [ForeignKey("CruiseServiceUnitsId")]
        public CruiseServiceUnit CruiseServiceUnitsFk { get; set; }
        public virtual int? ServiceName { get; set; }
        [ForeignKey("ServiceName")]
        public MasterAmenities ServiceNaFk { get; set; }
        public virtual bool PayOnSpot { get; set; }
        public virtual bool ReductionCanBeApplied { get; set; }
        public virtual bool Obligatory { get; set; }
        public virtual int? TenantRecipient { get; set; }
        public virtual string Lang { get; set; }
        public virtual bool Taxable { get; set; }
    }
}
