using Abp.Application.Services.Dto;
namespace CruisePMS.CruiseServiceGroups.Dtos
{
    public class CruiseServiceGroupsDto : EntityDto
    {
        public bool IsMainService { get; set; }
        public bool OnlyOneCanBeChoosen { get; set; }
        public int? ServiceGroupName { get; set; }

    }
}
