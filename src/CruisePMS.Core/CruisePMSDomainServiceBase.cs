using Abp.Domain.Services;

namespace CruisePMS
{
    public abstract class CruisePMSDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected CruisePMSDomainServiceBase()
        {
            LocalizationSourceName = CruisePMSConsts.LocalizationSourceName;
        }
    }
}
