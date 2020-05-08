using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.AgePolicies.Dtos
{
	public class CreateOrEditAgePoliciesDto : EntityDto<long?>
	{
		public int AgeFrom { get; set; }
		public int AgeTo { get; set; }
		public int GuestType { get; set; }
	}
}
