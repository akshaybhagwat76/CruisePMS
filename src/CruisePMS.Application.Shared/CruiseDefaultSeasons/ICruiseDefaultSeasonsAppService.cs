using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.CruiseDefaultSeasons.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CruisePMS.CruiseDefaultSeasons
{
    public interface ICruiseDefaultSeasonsAppService: IApplicationService
    {
        Task<PagedResultDto<GetCruiseDefaultSeasonsForViewDto>> GetAll(GetAllCruiseDefaultSeasonsInput input);

        Task<PagedResultDto<GetCruiseDefaultSeasonsForViewDto>> GetAllSavedDates();
        Task<GetCruiseDefaultSeasonsForViewDto> GetCruiseDefaultSeasonsForView(int id);

        Task<GetCruiseDefaultSeasonsForEditOutput> GetCruiseDefaultSeasonsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCruiseDefaultSeasonsDto input);


        Task CreateCruiseDefaultSeasons(string collection);

        Task Delete(EntityDto input);

        Task<PagedResultDto<GetCruiseDefaultSeasonsForViewDto>> GetAllCruiseDefaultSeasons(int dayNumber);
    }

}
