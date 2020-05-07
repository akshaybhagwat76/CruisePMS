using System.Threading.Tasks;
using Abp.Application.Services;
using CruisePMS.Editions.Dto;
using CruisePMS.MultiTenancy.Dto;

namespace CruisePMS.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}