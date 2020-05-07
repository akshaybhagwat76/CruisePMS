using Abp.Application.Services.Dto;
using System;

namespace CruisePMS.CruiseMasterAmenities.Dtos
{
    public class GetAllMasterAmenitiesesForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string DisplayNameFilter { get; set; }



    }
}