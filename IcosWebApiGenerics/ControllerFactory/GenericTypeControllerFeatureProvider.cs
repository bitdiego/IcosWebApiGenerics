using IcosWebApiGenerics.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.ControllerFactory
{
    public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var currentAssembly = typeof(GenericTypeControllerFeatureProvider).Assembly;
            var candidates = currentAssembly.GetExportedTypes().Where(x => x.GetCustomAttributes<GeneratedControllerAttribute>().Any());

            foreach (var candidate in candidates)
            {
                feature.Controllers.Add(
                    typeof(ValidatorController<>).MakeGenericType(candidate).GetTypeInfo()
                );
            }
        }
    }
}
