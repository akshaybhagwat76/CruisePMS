using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CruisePMS.Cruises.Dtos
{
    public class CreateOrEditCruisesDto : EntityDto<int?>
    {

        public int CruiseDuration { get; set; }


        public int CruiseStartPort { get; set; }


        public int CruiseEndPort { get; set; }


        [Required]
        public string Cruise_Airport { get; set; }


        public bool CruiseIsEnabled { get; set; }


        public bool CruiseIsEnabledB2B { get; set; }


        public bool DisableForApi { get; set; }


        public decimal StandardDeposit { get; set; }


        public int DepositType { get; set; }


        public string CheckIn { get; set; }


        public string Checkout { get; set; }

        public int? CruiseShipsId { get; set; }

        public int? CruiseThemesId { get; set; }

        public int? CruiseServicesId { get; set; }

        public int? CruiseItinerariesId { get; set; }

        public bool VirtualCruise { get; set; }

        //public bool OneWay { get; set; }


        public int CruiseYear { get; set; }

        public bool FreeInternet { get; set; }

        public bool TransferIncluded { get; set; }
        public int CruiseOperatorId { get; set; }

        public string BookingEmail { get; set; }
    }
}
