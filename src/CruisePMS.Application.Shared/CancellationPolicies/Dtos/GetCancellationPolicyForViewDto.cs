namespace CruisePMS.CancellationPolicies.Dtos
{
    public class GetCancellationPolicyForViewDto
    {
        public CancellationPolicyDto CancellationPolicy { get; set; }

        public string CruiseServicesServiceName { get; set; }

        public string CruiseMasterAmenitiesDisplayName { get; set; }

        public string CruiseMasterAmenitiesDisplayName2 { get; set; }
    }
}
