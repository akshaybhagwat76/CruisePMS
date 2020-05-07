using Abp.AspNetCore.Mvc.ViewComponents;

namespace CruisePMS.Web.Public.Views
{
    public abstract class CruisePMSViewComponent : AbpViewComponent
    {
        protected CruisePMSViewComponent()
        {
            LocalizationSourceName = CruisePMSConsts.LocalizationSourceName;
        }
    }
}