using Abp.AspNetCore.Mvc.Views;

namespace CruisePMS.Web.Views
{
    public abstract class CruisePMSRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected CruisePMSRazorPage()
        {
            LocalizationSourceName = CruisePMSConsts.LocalizationSourceName;
        }
    }
}
