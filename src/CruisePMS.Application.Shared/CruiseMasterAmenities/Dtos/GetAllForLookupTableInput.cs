﻿using Abp.Application.Services.Dto;

namespace CruisePMS.CruiseMasterAmenities.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}