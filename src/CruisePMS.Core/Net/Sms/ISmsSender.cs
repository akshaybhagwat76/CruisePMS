using System.Threading.Tasks;

namespace CruisePMS.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}