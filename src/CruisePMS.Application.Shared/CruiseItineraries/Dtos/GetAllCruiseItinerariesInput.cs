using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseItineraries.Dtos
{
	public class GetAllCruiseItinerariesInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }

		public int? MaxSeasonYearFilter { get; set; }
		public int? MinSeasonYearFilter { get; set; }

		public string ItineraryNameFilter { get; set; }

		public string ItineraryCodeFilter { get; set; }



	}
}
