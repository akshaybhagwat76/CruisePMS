using Abp.Application.Services.Dto;
namespace CruisePMS.AccountPayments.Dtos
{
    public class GetAllAccountPaymentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
