using System.Collections.Generic;

namespace CruisePMS.DynamicEntityParameters
{
    public interface IDynamicEntityParameterDefinitionAppService
    {
        List<string> GetAllAllowedInputTypeNames();

        List<string> GetAllEntities();
    }
}
