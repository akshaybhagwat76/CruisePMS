using Abp.Application.Services.Dto;
using System;
namespace CruisePMS.Cruises.Dtos
{
    public class CruisesDto : EntityDto
    {
        public int CruiseDuration { get; set; }

        public int CruiseYear { get; set; }

        public string CruiseStartPort { get; set; }

        public string CruiseEndPort { get; set; }

        public bool CruiseIsEnabled { get; set; }

        public bool CruiseIsEnabledB2B { get; set; }

        public bool DisableForApi { get; set; }
        public bool VirtualCruise { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime Checkout { get; set; }

        public int? CruiseShipsId { get; set; }
        public int? CruiseThemesId { get; set; }
        public int? CruiseServicesId { get; set; }
        public int? CruiseItinerariesId { get; set; }
        public bool IsDeleted { get; set; }
        public string BookingEmail { get; set; }
        public bool TransferIncluded { get; set; }
        public int CruiseOperatorId { get; set; }
    }
}
