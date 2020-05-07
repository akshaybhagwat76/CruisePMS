using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CruisePMS.Startup
{
    [DependsOn(typeof(CruisePMSCoreModule))]
    public class CruisePMSGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CruisePMSGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}