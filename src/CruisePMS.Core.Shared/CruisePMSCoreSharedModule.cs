using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CruisePMS
{
    public class CruisePMSCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CruisePMSCoreSharedModule).GetAssembly());
        }
    }
}