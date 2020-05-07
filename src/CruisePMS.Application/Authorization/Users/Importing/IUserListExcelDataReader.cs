using System.Collections.Generic;
using CruisePMS.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace CruisePMS.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
