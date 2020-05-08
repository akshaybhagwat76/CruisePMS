using Abp.Application.Services.Dto;
namespace CruisePMS.Common.Dto
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int Parentid { get; set; }
        public int ContractId { get; set; }
        public long ReservationId { get; set; }
        public string CabinIdentificator { get; set; }
        public int DepartureId { get; set; }

    }
}
