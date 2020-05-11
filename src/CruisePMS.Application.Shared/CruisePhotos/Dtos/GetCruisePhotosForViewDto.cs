using CruisePMS.CruisePhotos.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruisePhotos.Dtos
{
   public class GetCruisePhotosForViewDto
    {
        public CruisePhotosDto CruisePhotos { get; set; }

        public string CruiseMasterAmenitiesDisplayName { get; set; }


    }
}
