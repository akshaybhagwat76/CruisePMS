using Abp.Application.Services.Dto;
using System;
namespace CruisePMS.Clients.Dtos
{
    public class GetAllClientsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxClientDOBFilter { get; set; }
        public DateTime? MinClientDOBFilter { get; set; }

        public DateTime? MaxWeddingAniversaryFilter { get; set; }
        public DateTime? MinWeddingAniversaryFilter { get; set; }

        public string ClientCountryFilter { get; set; }

        public string ClientGoogleAddressFilter { get; set; }

        public string ClientEmailFilter { get; set; }

        public string ContactCellPhoneFilter { get; set; }

        public DateTime? MaxIssuedFilter { get; set; }
        public DateTime? MinIssuedFilter { get; set; }

        public DateTime? MaxExpirationFilter { get; set; }
        public DateTime? MinExpirationFilter { get; set; }

        public string CountryResidenceFilter { get; set; }

        public string MedicalIssuesFilter { get; set; }

        public string FoodIssuesFilter { get; set; }

        public string ClientPasswordFilter { get; set; }

        public int? MaxIsClientLoginWithFilter { get; set; }
        public int? MinIsClientLoginWithFilter { get; set; }

        public int? MaxLoginFailAttemptFilter { get; set; }
        public int? MinLoginFailAttemptFilter { get; set; }

        public int IsLockedFilter { get; set; }


        public string CruiseMasterAmenitiesDisplayNameFilter { get; set; }

        public string CruiseMasterAmenitiesDisplayName2Filter { get; set; }

        public string CruiseMasterAmenitiesDisplayName3Filter { get; set; }
    }
}
