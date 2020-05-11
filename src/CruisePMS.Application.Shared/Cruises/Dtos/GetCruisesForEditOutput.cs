using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.Cruises.Dtos
{
	public class GetCruisesForEditOutput
	{
		public CreateOrEditCruisesDto Cruises { get; set; }

		public string CruiseShipsCruiseShipName { get; set; }

		public string CruiseThemesCruiseThemeName { get; set; }

		public string CruiseServicesServiceName { get; set; }

		public string CruiseItinerariesItineraryName { get; set; }


	}
}