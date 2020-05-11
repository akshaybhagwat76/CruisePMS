namespace CruisePMS.CruiseReductions.Dtos
{
    public class GetCruiseReductionsForViewDto
    {
        public CruiseReductionsDto CruiseReductions { get; set; }
        public string CruiseServicesServiceName { get; set; }
        public string AgePoliciesTenantId { get; set; }
        public string PassengerNoName { get; set; }
        public string AgePolicy { get; set; }
        public string ReductionName { get; set; }
    }
}
