using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseItineraries.Dtos
{
	public class CreateOrEditCruiseItinerariesDto : EntityDto
	{

		public string ItineraryName { get; set; }


		public string ItineraryCode { get; set; }


		public string ItineraryMap { get; set; }

		public string Description { get; set; }

		public int OnBoardService { get; set; }
	}
}
