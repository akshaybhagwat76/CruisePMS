using System.Collections.Generic;
using CruisePMS.Authorization.Users.Dto;
using CruisePMS.Dto;

namespace CruisePMS.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}