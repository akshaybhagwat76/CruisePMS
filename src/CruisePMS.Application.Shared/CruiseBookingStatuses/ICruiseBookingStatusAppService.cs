using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.BookingStatuses.Dtos;
using CruisePMS.Dto;
using System.Threading.Tasks;
namespace CruisePMS.CruiseBookingStatuses
{
    public interface ICruiseBookingStatusAppService : IApplicationService
    {
        Task<PagedResultDto<GetCruiseBookingStatusForViewDto>> GetAll(GetAllCruiseBookingStatusInput input);

        Task<GetCruiseBookingStatusForViewDto> GetCruiseBookingStatusForView(int id);

        Task<GetCruiseBookingStatusForEditOutput> GetCruiseBookingStatusForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCruiseBookingStatusDto input);

        Task Delete(EntityDto input);

        //Task<FileDto> GetCruiseBookingStatusToExcel(GetAllCruiseBookingStatusForExcelInput input);


    }
}
