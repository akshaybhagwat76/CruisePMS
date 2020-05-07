using Abp.Application.Services.Dto;

namespace CruisePMS.CruiseTenantTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
