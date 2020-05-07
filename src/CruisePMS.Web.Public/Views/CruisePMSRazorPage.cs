using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace CruisePMS.Web.Public.Views
{
    public abstract class CruisePMSRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected CruisePMSRazorPage()
        {
            LocalizationSourceName = CruisePMSConsts.LocalizationSourceName;
        }
    }
}
