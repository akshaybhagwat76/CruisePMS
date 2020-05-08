using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.AmenityStorages
{
    [Table("AppAmenityStorage")]
    [Audited]
    public class AmenityStorage : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        public virtual long SourceId { get; set; }

        //public virtual byte[] IconPhoto { get; set; }

        public virtual long? SectionId { get; set; }

        public virtual string Filter { get; set; }

        public virtual int? ReOrder { get; set; }

        public virtual int MasterAmenitiesId { get; set; }

        [ForeignKey("MasterAmenitiesId")]
        public MasterAmenities MasterAmenitiesFk { get; set; }
    }
}
