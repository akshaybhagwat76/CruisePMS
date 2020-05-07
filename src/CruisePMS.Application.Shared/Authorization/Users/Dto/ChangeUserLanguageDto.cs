using System.ComponentModel.DataAnnotations;

namespace CruisePMS.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
