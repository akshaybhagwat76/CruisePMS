using Abp.Application.Services.Dto;
namespace CruisePMS.CruiseDefaultSeasons.Dtos
{
    public class GetAllCruiseDefaultSeasonsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int DayNumber { get; set; }
    }
}
