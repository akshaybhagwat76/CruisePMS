using Abp.Application.Services.Dto;
namespace CruisePMS.AgePolicies.Dtos
{
	public class AgePoliciesDto : EntityDto<long>
	{
		public int AgeFrom { get; set; }
		public int AgeTo { get; set; }
		public int GuestType { get; set; }
	}
}
