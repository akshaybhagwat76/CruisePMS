using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseReductions.Dtos
{
    public class CreateOrEditCruiseReductionsDto : EntityDto<int?>
    {

        public int CruiseReduction { get; set; }


        public decimal ReductionAmount { get; set; }


        public int ReductionTYpe { get; set; }


        public int ActivateOn { get; set; }


        public int ServicesId { get; set; }

        public long AgePoliciesId { get; set; }


    }
}
