using Abp.Application.Services.Dto;

namespace CruisePMS.CruiseServiceGroups.Dtos
{
    public class GetAllCruiseServiceGroupsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }


        public string CruiseMasterAmenitiesDisplayNameFilter { get; set; }


    }
}