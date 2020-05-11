using Abp.Application.Services.Dto;
namespace CruisePMS.CruiseFares.Dtos
{
    public class GetAllCruiseFaresInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxActivateDaysBeforeDepartureFilter { get; set; }
        public int? MinActivateDaysBeforeDepartureFilter { get; set; }

        public int CruiseId { get; set; }
        public string CruisesTenantIdFilter { get; set; }

        public string CruiseMasterAmenitiesDisplayNameFilter { get; set; }

        public int ShipId { get; set; }

        public int SourceId { get; set; }

    }
}
