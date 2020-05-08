using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.AmenityStorages.Dtos
{
    public class GetAllCruiseAmenitiesStorageInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int SourceId { get; set; }

        public int SectionId { get; set; }

        public string CruiseMasterAmenitiesDisplayNameFilter { get; set; }
    }
}
