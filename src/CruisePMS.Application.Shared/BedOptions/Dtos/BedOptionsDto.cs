using Abp.Application.Services.Dto;
namespace CruisePMS.BedOptions.Dtos
{
    public class BedOptionsDto : EntityDto
    {
        public int BedCapacity { get; set; }
        public int? BedOptionName { get; set; }
    }
}
