using System.Collections.Generic;
using CruisePMS.Authorization.Users.Importing.Dto;
using CruisePMS.Dto;

namespace CruisePMS.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
