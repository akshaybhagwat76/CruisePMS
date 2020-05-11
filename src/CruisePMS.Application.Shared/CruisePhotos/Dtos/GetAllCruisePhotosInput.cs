using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruisePhotos.Dtos
{
    public class GetAllCruisePhotosInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string CruiseMasterAmenitiesDisplayNameFilter { get; set; }

        public int SourceId { get; set; }
        public string SourceName { get; set; }



    }
}
