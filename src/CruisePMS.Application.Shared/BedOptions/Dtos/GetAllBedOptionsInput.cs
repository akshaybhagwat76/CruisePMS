using Abp.Application.Services.Dto;
namespace CruisePMS.BedOptions.Dtos
{
    public class GetAllBedOptionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxBedCapacityFilter { get; set; }
        public int? MinBedCapacityFilter { get; set; }
        public string CruiseMasterAmenitiesDisplayNameFilter { get; set; }
    }
}
