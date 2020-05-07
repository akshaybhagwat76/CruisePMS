using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CruisePMS
{
    [DependsOn(typeof(CruisePMSXamarinSharedModule))]
    public class CruisePMSXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CruisePMSXamarinAndroidModule).GetAssembly());
        }
    }
}