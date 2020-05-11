namespace CruisePMS.CruiseFares.Dtos
{
    public class GetCruiseFaresForEditOutput
    {
        public CreateOrEditCruiseFaresDto CruiseFares { get; set; }
        public string CruisesTenantId { get; set; }
        public string CruiseMasterAmenitiesDisplayName { get; set; }
    }
}
