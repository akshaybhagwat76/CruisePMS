using Abp.AutoMapper;
using CruisePMS.Organizations.Dto;

namespace CruisePMS.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}