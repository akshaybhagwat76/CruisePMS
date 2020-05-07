using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CruisePMS
{
    public class CruisePMSClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CruisePMSClientModule).GetAssembly());
        }
    }
}
