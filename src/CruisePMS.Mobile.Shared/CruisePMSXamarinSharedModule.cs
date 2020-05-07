using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CruisePMS
{
    [DependsOn(typeof(CruisePMSClientModule), typeof(AbpAutoMapperModule))]
    public class CruisePMSXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CruisePMSXamarinSharedModule).GetAssembly());
        }
    }
}