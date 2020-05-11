using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseItineraries.Dtos
{
    public class GetCruiseItinerariesForEditOutput
    {
        public CreateOrEditCruiseItinerariesDto CruiseItineraries { get; set; }


    }
    public class GetAllOnBoardService
    {
        public List<OnBoardService> OnBoardServices { get; set; } = new List<OnBoardService>();
    }
    public class OnBoardService
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
}
