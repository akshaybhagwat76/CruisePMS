using Abp.Application.Services.Dto;
namespace CruisePMS.CancellationPolicies.Dtos
{
	public class GetAllCancellationPolicyInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }

		public string CancellationPercentageFilter { get; set; }

		public int? MaxDaysFromFilter { get; set; }
		public int? MinDaysFromFilter { get; set; }

		public int? MaxDaysToFilter { get; set; }
		public int? MinDaysToFilter { get; set; }


		public string CruiseServicesServiceNameFilter { get; set; }

		public string CruiseMasterAmenitiesDisplayNameFilter { get; set; }

		public string CruiseMasterAmenitiesDisplayName2Filter { get; set; }


	}
}
