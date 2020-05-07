using System.Collections.Generic;
using Abp;
using CruisePMS.Chat.Dto;
using CruisePMS.Dto;

namespace CruisePMS.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
