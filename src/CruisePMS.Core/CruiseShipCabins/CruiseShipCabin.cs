using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.BedOptions;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.CruiseShips;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseShipCabins
{
    [Table("AppCruiseShipCabin")]
    [Audited]
    public class CruiseShipCabin : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        public virtual string CabinNo { get; set; }

        public virtual int CabinArea { get; set; }

        public virtual int ShipCabinSortOrder { get; set; }

        public virtual bool porthole { get; set; }

        public virtual bool window { get; set; }

        public virtual bool Balcony { get; set; }


        public virtual int? CruiseShipsId { get; set; }

        [ForeignKey("CruiseShipsId")]
        public CruiseShip CruiseShipsFk { get; set; }

        public virtual int? BedOptionsId { get; set; }
        public virtual int? CabinSort { get; set; }



        [ForeignKey("BedOptionsId")]
        public BedOption BedOptionsFk { get; set; }

        public virtual int? CabinCategoryId { get; set; }

        [ForeignKey("CabinCategoryId")]
        public MasterAmenities CabinCategoryFk { get; set; }

        public virtual int? CruiseShipDecksId { get; set; }

        [ForeignKey("CruiseShipDecksId")]
        public MasterAmenities CruiseShipDecksFk { get; set; }

        public virtual int? CabinLocationId { get; set; }

        [ForeignKey("CabinLocationId")]
        public MasterAmenities CabinLocationFk { get; set; }

        public virtual int CabinTypeID { get; set; }

        [ForeignKey("CabinTypeID")]
        public MasterAmenities CabinTypeFk { get; set; }

        public virtual bool AllowAsSingle { get; set; }

        public virtual bool DisableCabin { get; set; }

    }
}
