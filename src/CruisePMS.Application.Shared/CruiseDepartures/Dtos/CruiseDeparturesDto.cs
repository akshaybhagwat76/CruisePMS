using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseDepartures.Dtos
{
	public class CruiseDeparturesDto : EntityDto
	{
		public int DepartureYear { get; set; }

		public string SeasonGroup { get; set; }

		public DateTime DepartureDate { get; set; }

		public bool IsDeleted { get; set; }


		public int? CruisesId { get; set; }
	}
}
