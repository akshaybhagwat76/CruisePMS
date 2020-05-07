using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using CruisePMS.Queries.Container;

namespace CruisePMS.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}