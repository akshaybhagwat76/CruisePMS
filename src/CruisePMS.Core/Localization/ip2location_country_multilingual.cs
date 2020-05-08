using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.Localization
{
    [Table("ip2location_country_multilingual")]
    public class ip2location_country_multilingual : Entity
    {
        public string lang { get; set; }
        public string lang_name { get; set; }
        public string country_alpha2_code { get; set; }
        public string country_alpha3_code { get; set; }
        public string country_numeric_code { get; set; }
        public string country_name { get; set; }
    }
}
