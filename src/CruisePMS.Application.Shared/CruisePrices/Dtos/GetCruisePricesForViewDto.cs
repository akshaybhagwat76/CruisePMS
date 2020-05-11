namespace CruisePMS.CruisePrices.Dtos
{
	public class GetCruisePricesForViewDto
	{
		public CruisePricesDto CruisePrices { get; set; }

		public string CruiseFaresFareName { get; set; }

		public string CruiseDeparturesDepartureDate { get; set; }

		public string CruiseShipCabinsCabinCategoryId { get; set; }

		public string CruiseServicesServiceName { get; set; }

		public string CruiseShipsCruiseShipName { get; set; }

		public string SeasonGroup { get; set; }
		public string CruiseServicesServiceUnit { get; set; }
	}
}
