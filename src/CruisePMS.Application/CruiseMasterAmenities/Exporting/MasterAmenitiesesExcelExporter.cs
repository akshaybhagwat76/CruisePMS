using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using CruisePMS.DataExporting.Excel.NPOI;
using CruisePMS.CruiseMasterAmenities.Dtos;
using CruisePMS.Dto;
using CruisePMS.Storage;

namespace CruisePMS.CruiseMasterAmenities.Exporting
{
    public class MasterAmenitiesesExcelExporter : NpoiExcelExporterBase, IMasterAmenitiesesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MasterAmenitiesesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMasterAmenitiesForViewDto> masterAmenitieses)
        {
            return CreateExcelPackage(
                "MasterAmenitieses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("MasterAmenitieses"));

                    AddHeader(
                        sheet,
                        L("DisplayName")
                        );

                    AddObjects(
                        sheet, 2, masterAmenitieses,
                        _ => _.MasterAmenities.DisplayName
                        );

					
					
                });
        }
    }
}
