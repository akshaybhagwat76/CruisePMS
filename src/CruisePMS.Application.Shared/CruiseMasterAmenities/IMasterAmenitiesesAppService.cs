using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.CruiseMasterAmenities.Dtos;
using CruisePMS.Dto;


namespace CruisePMS.CruiseMasterAmenities
{
    public interface IMasterAmenitiesesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMasterAmenitiesForViewDto>> GetAll(GetAllMasterAmenitiesesInput input);

        Task<GetMasterAmenitiesForViewDto> GetMasterAmenitiesForView(int id);

		Task<GetMasterAmenitiesForEditOutput> GetMasterAmenitiesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditMasterAmenitiesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetMasterAmenitiesesToExcel(GetAllMasterAmenitiesesForExcelInput input);

		
    }
}