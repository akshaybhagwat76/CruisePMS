using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.Localization
{
    [Table("ip2location_city_multilingual")]
    public class ip2location_city_multilingual : Entity
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

    public class GetCity
    {
        public int Id { get; set; }
        public string lang_city_name { get; set; }
    }
}
