using Abp.Application.Services.Dto;

namespace CruisePMS.AgePolicies.Dtos
{
    public class GetAllAgePoliciesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string CruiseMasterAmenitiesDisplayNameFilter { get; set; }
    }
}