using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseServiceGroups.Dtos
{
	public class CreateOrEditCruiseServiceGroupsDto : EntityDto<int?>
	{

		public bool IsMainService { get; set; }


		public bool OnlyOneCanBeChoosen { get; set; }


		public DateTime? StartDate { get; set; }


		public DateTime? StopDate { get; set; }


		public int? ServiceGroupName { get; set; }


	}
}
