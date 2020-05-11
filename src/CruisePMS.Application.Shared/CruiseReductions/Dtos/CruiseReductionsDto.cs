using Abp.Application.Services.Dto;
namespace CruisePMS.CruiseReductions.Dtos
{
    public class CruiseReductionsDto : EntityDto
    {
        public int CruiseReduction { get; set; }
        public decimal ReductionAmount { get; set; }
        public int ActivateOn { get; set; }
        public int ServicesId { get; set; }
        public long AgePoliciesId { get; set; }
        public string ServiceName { get; set; }
        public string ReductionType { get; set; }
    }
}
