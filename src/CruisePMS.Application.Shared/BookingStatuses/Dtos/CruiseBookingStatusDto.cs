using Abp.Application.Services.Dto;
namespace CruisePMS.BookingStatuses.Dtos
{
    public class CruiseBookingStatusDto : EntityDto
    {
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public string StatusShort { get; set; }
    }
}
