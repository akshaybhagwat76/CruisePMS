using Abp.Application.Services.Dto;
using System;
namespace CruisePMS.CruiseDefaultSeasons.Dtos
{
	public class CruiseDefaultSeasonsDto : EntityDto
	{
		public int DepartureYear { get; set; }

		public string SeasonGroup { get; set; }

		public DateTime DepartureDate { get; set; }

	}
}
