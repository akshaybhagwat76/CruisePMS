using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseFares.Dtos
{
	public class CruiseFaresDto : EntityDto
	{
		public bool IsMainFare { get; set; }

		public DateTime? FareStartDate { get; set; }

		public DateTime? FareEndDate { get; set; }

		public decimal Discount { get; set; }

		public int ActivateDaysBeforeDeparture { get; set; }


		public int? CruisesId { get; set; }

		public int? FareName { get; set; }
		public bool FullPaymentRequired { get; set; }

		public DateTime? DepartureDate { get; set; }
		public long? DepartureId { get; set; }
	}
}
