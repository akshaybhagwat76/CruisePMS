using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using CruisePMS.Configure;
using CruisePMS.Startup;
using CruisePMS.Test.Base;

namespace CruisePMS.GraphQL.Tests
{
    [DependsOn(
        typeof(CruisePMSGraphQLModule),
        typeof(CruisePMSTestBaseModule))]
    public class CruisePMSGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CruisePMSGraphQLTestModule).GetAssembly());
        }
    }
}