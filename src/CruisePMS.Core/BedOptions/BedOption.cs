using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.BedOptions.Dtos;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.BedOptions
{
    [Table("AppBedOptions")]
    [Audited]
    public class BedOption : Entity
    {
        [Range(BedOptionsConsts.MinBedCapacityValue, BedOptionsConsts.MaxBedCapacityValue)]
        public virtual int BedCapacity { get; set; }


        public virtual int? BedOptionName { get; set; }

        [ForeignKey("BedOptionName")]
        public MasterAmenities BedOptionNaFk { get; set; }

    }
}
