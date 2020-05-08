using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.Clients.Dtos
{
    public class ClientsCruiseMasterAmenitiesLookupTableDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }
    }

    public class ClientsLookupTableDto
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelectDisable { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public bool IsReservationHolder { get; set; }

    }

    public class ClientsDetails
    {
        public long Id { get; set; }
        public string CountryName { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public bool IsSelectDisable { get; set; }
        public DateTime DOB { get; set; }
        public bool IsReservationHolder { get; set; }
        public int Age { get; set; }
    }

}
