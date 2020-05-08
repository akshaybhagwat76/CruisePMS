using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.ContractCommissions.Dtos
{
    public class GetCruiseContractCommissionsForEditOutput
    {
        public CreateOrEditCruiseContractCommissionsDto CruiseContractCommissions { get; set; }

        public string CruisesCruiseName { get; set; }

        public string CruiseShipsCruiseShipName { get; set; }

        public string CruiseContractContractName { get; set; }

        public string CruiseServicesServiceName { get; set; }


    }
}

