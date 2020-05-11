using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
namespace CruisePMS.CruisePhotos.Dtos
{
    public class CreateOrEditCruisePhotosDto : EntityDto<long?>
    {

        [Required]
        public string PhotoSource { get; set; }


        public byte[] Photo1 { get; set; }


        public byte[] Photo2 { get; set; }


        public byte[] Photo3 { get; set; }


        public byte[] Photo4 { get; set; }


        public byte[] Photo5 { get; set; }


        public int PhotoSourceId { get; set; }


        public int PhotoNameId { get; set; }


    }

    public class SaveCruisePhotos
    {
        public string PhotoSource { get; set; }
        public int PhotoSourceId { get; set; }
        public int PhotoNameId { get; set; }

        public string Filbase64 { get; set; }

    }
}
