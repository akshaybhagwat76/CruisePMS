using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CruisePMS
{
    [DependsOn(typeof(CruisePMSCoreSharedModule))]
    public class CruisePMSApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CruisePMSApplicationSharedModule).GetAssembly());
        }
    }
}