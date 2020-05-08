using Abp.Application.Services.Dto;
namespace CruisePMS.AmenityStorages.Dtos
{
    public class CreateOrEditCruiseAmenitiesStorageDto : EntityDto<long?>
    {
        public long SourceId { get; set; }
        public long SectionId { get; set; }
        public int MasterAmenitiesId { get; set; }
    }

    public class SaveCruiseAmenitiesStorageDto
    {
        public long SourceId { get; set; }
        public int MasterAmenities { get; set; }
        public long SectionId { get; set; }
        public string Filter { get; set; }
    }
}
