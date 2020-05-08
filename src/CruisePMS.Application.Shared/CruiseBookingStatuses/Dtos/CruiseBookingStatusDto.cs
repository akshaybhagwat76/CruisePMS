using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseBookingStatuses.Dtos
{
	public class CruiseBookingStatusDto : EntityDto
	{
		public string StatusName { get; set; }

		public string StatusColor { get; set; }

		public string StatusShort { get; set; }



	}
}
