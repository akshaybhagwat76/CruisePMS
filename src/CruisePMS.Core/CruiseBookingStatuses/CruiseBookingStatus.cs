using Abp.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseBookingStatuses
{
    [Table("AppCruiseBookingStatus")]
    [Audited]
    public class CruiseBookingStatus : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        public virtual string StatusName { get; set; }

        public virtual string StatusColor { get; set; }

        public virtual string StatusShort { get; set; }


    }
}
