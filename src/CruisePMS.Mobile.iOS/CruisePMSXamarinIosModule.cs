using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CruisePMS
{
    [DependsOn(typeof(CruisePMSXamarinSharedModule))]
    public class CruisePMSXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CruisePMSXamarinIosModule).GetAssembly());
        }
    }
}