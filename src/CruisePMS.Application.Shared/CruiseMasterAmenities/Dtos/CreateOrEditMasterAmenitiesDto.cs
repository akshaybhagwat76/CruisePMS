
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CruisePMS.CruiseMasterAmenities.Dtos
{
    public class CreateOrEditMasterAmenitiesDto : EntityDto<int?>
    {

		public string Code { get; set; }


		public string DisplayName { get; set; }


		public int ParentId { get; set; }


	}
}