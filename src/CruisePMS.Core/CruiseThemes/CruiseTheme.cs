using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseThemes
{
    [Table("AppCruiseThemes")]
    [Audited]
    public class CruiseTheme : Entity
    {

        public virtual string CruiseThemeDescription { get; set; }


        public virtual int? CruiseThemeName { get; set; }

        [ForeignKey("CruiseThemeName")]
        public MasterAmenities CruiseThemeNaFk { get; set; }

    }
}
