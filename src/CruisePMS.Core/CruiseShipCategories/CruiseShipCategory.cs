using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.CruiseShipCategories
{
    [Table("AppCruiseShipCategory")]
    public class CruiseShipCategory : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual int CruiseShipCategoryName { get; set; }
    }
}
