using System.Collections.Generic;

namespace CruisePMS.Contracts.Dtos
{
    public class GetCruiseContractForViewDto
    {
        public CruiseContractDto CruiseContract { get; set; }
    }
    public class CreateRecepientTimeRecordsDto
    {
        public long TenantId { get; set; }
        public string TenantName { get; set; }
        public long CurrentLoggedInUserId { get; set; }
        public string CurrentLoggedInUserName { get; set; }


    }
    public class GetAllCruiseOpratorsTenants
    {
        public List<CruiseOpratorsTenant> cruiseOpratorsTenant { get; set; }
    }
    public class CruiseOpratorsTenant
    {
        public long TenantId { get; set; }
        public string TenantName { get; set; }
        public bool IsCruiseOprator { get; set; }
    }

}
