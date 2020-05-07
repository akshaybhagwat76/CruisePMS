using Microsoft.Extensions.Configuration;

namespace CruisePMS.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
