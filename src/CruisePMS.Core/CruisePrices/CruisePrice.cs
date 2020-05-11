using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.CruiseFares;
using CruisePMS.CruiseServices;
using CruisePMS.CruiseShipCabins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruisePrices
{
    [Table("CruisePrices")]
    [Audited]
    public class CruisePrice : Entity, IMayHaveTenant, IFullAudited
    {
        public int? TenantId { get; set; }


        public virtual decimal UnitPrice { get; set; }

        public virtual string CurrencyId { get; set; }


        public virtual int? CruiseFaresId { get; set; }

        [ForeignKey("CruiseFaresId")]
        public CruiseFare CruiseFaresFk { get; set; }



        public virtual int? CruiseShipCabinsId { get; set; }

        [ForeignKey("CruiseShipCabinsId")]
        public CruiseShipCabin CruiseShipCabinsFk { get; set; }

        public virtual int? CruiseServicesId { get; set; }

        public virtual int? ServiceUnitId { get; set; }


        [ForeignKey("CruiseServicesId")]
        public CruiseService CruiseServicesFk { get; set; }




        public virtual string SeasonGroup { get; set; }
        public virtual int? CabinTypeId { get; set; }
        public virtual int? DeckId { get; set; }

        public virtual int? PriceYear { get; set; }
        public virtual long CruiseId { get; set; }
        public virtual int? ShipId { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

    }
}
