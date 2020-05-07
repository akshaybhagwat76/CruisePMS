using Abp.Application.Services.Dto;
using System;
namespace CruisePMS.CruiseDefaultSeasons.Dtos
{
    public class CreateOrEditCruiseDefaultSeasonsDto : EntityDto<int?>
    {
        public int DepartureYear { get; set; }
        public string SeasonGroup { get; set; }
        public DateTime DepartureDate { get; set; }
    }
    public class CreateCruiseDefaultSeasonLoop
    {
        public int DepartureYear { get; set; }
        public string SeasonGroup { get; set; }
        public string DepartureDate { get; set; }
    }
}
