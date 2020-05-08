using Abp.Application.Services.Dto;
namespace CruisePMS.AmenityStorages.Dtos
{
    public class CruiseAmenitiesStorageDto : EntityDto<long>
    {
        public long SourceId { get; set; }
        public int MasterAmenitiesId { get; set; }
    }
}
