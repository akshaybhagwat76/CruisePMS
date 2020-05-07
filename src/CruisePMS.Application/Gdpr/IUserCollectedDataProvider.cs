using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using CruisePMS.Dto;

namespace CruisePMS.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
