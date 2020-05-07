using System.ComponentModel.DataAnnotations;

namespace CruisePMS.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}