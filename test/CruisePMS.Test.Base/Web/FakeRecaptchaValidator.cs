using System.Threading.Tasks;
using CruisePMS.Security.Recaptcha;

namespace CruisePMS.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
