using Abp.Application.Services.Dto;
namespace CruisePMS.CancellationPolicies.Dtos
{
    public class CancellationPolicyDto:EntityDto
    {
		public string CancellationPercentage { get; set; }

		public int DaysFrom { get; set; }

		public int DaysTo { get; set; }

		public int SeasonYear { get; set; }


		public int? CruiseServicesId { get; set; }

		public int? CancellationPreText { get; set; }

		public int? CancellationPostText { get; set; }
	}
}
