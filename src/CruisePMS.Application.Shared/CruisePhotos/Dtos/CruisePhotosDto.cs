using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;

namespace CruisePMS.CruisePhotos.Dtos
{
    public class CruisePhotosDto : EntityDto<long>
    {
        public string PhotoSource { get; set; }
        public int PhotoNameId { get; set; }
        public string Photo1 { get; set; }



    }
    public class UploadFileInput
    {
        public string PhotoSource { get; set; }
        public int PhotoNameId { get; set; }
        public int PhotoSourceId { get; set; }

        public IFormFile File { get; set; }

    }
}
