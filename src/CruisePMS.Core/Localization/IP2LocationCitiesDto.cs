using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.Localization
{
    public class IP2LocationCitiesDto : EntityDto
    {
        public int Id { get; set; }

        public string country_alpha2_code { get; set; }
        public string country_numeric_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city_name { get; set; }
        public string lang_code { get; set; }
        public string lang_name { get; set; }
        public string lang_region_name { get; set; }
        public string lang_city_name { get; set; }
    }
}
