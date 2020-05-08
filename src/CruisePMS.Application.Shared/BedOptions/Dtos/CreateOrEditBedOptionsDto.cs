using Abp.Application.Services.Dto;
namespace CruisePMS.BedOptions.Dtos
{
	public class CreateOrEditBedOptionsDto : EntityDto<int?>
	{
		public int BedCapacity { get; set; }
		public int? BedOptionName { get; set; }
	}
}
