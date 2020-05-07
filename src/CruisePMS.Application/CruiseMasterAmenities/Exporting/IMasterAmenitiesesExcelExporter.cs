using System.Collections.Generic;
using CruisePMS.CruiseMasterAmenities.Dtos;
using CruisePMS.Dto;

namespace CruisePMS.CruiseMasterAmenities.Exporting
{
    public interface IMasterAmenitiesesExcelExporter
    {
        FileDto ExportToFile(List<GetMasterAmenitiesForViewDto> masterAmenitieses);
    }
}