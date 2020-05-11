using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseReductions.Dtos
{
	public class GetAllCruiseReductionsInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }

		public int? MaxReductionTYpeFilter { get; set; }
		public int? MinReductionTYpeFilter { get; set; }


		public string CruiseServicesServiceNameFilter { get; set; }

		public string AgePoliciesTenantIdFilter { get; set; }


	}
}
