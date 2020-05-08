namespace CruisePMS.CancellationPolicies.Dtos
{
    public class GetCancellationPolicyForEditOutput
    {
        public CreateOrEditCancellationPolicyDto CancellationPolicy { get; set; }
        public string CruiseServicesServiceName { get; set; }
        public string CruiseMasterAmenitiesDisplayName { get; set; }
        public string CruiseMasterAmenitiesDisplayName2 { get; set; }
    }
}
