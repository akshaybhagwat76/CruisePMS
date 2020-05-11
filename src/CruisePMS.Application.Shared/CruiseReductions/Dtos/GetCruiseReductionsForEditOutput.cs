using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseReductions.Dtos
{
    public class GetCruiseReductionsForEditOutput
    {
        public CreateOrEditCruiseReductionsDto CruiseReductions { get; set; }

        public string CruiseServicesServiceName { get; set; }

        public string AgePoliciesTenantId { get; set; }


    }

    public class GetReductionsScreenDto
    {
        public List<ReductionsScreen> reductionsScreen { get; set; } = new List<ReductionsScreen>();

    }
    public class ReductionsScreen
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string PassangerDisplayName { get; set; }
        public int PassangerNo { get; set; }

        public decimal ReductionAmount { get; set; }
        public string Age { get; set; }
        public int ReductionId { get; set; }
        public string ReductionName { get; set; }
        public List<ReductionServiceGroup> reductionServiceGroup { get; set; } = new List<ReductionServiceGroup>();
        public string ReductionType { get; set; }
        public bool AddBlankRow { get; set; }

        public long AgePolicyId { get; set; }
    }

    public class ReductionServiceGroup
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }

    public class SelectReductionServices
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public bool ReductionCanBeApplied { get; set; }
        public string Lang { get; set; }

    }

    public class GetReductionName
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Lang { get; set; }


    }
}
