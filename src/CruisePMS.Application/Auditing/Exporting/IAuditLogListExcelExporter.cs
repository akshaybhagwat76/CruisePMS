using System.Collections.Generic;
using CruisePMS.Auditing.Dto;
using CruisePMS.Dto;

namespace CruisePMS.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
