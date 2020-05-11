using Abp.Application.Services.Dto;
namespace CruisePMS.CruisePrices.Dtos
{
    public class GetAllCruisePricesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }


        public string CruiseFaresFareNameFilter { get; set; }

        public string CruiseDeparturesDepartureDateFilter { get; set; }

        public string CruiseShipCabinsCabinCategoryIdFilter { get; set; }

        public string CruiseServicesServiceNameFilter { get; set; }

        public string CruiseShipsCruiseShipNameFilter { get; set; }

        //public int FareId { get; set; }
        public int CruiseId { get; set; }
    }
}
