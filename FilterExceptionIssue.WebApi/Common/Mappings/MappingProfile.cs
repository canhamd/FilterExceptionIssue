using AutoMapper;
using System.Reflection;

namespace FilterExceptionIssue.WebApi.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMapFromMappingsFromAssembly(Assembly.GetExecutingAssembly());

            ApplyMapToMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMapFromMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var mapFromInterfaces = type.GetInterfaces().Where(i => i.Name == "IMapFrom`1");

                foreach (var mapFromInterface in mapFromInterfaces)
                {
                    var genericType = mapFromInterface.GenericTypeArguments[0];

                    var genericTypeName = $"{genericType.Namespace}.{genericType.Name}";

                    var implementedMethodName = $"{mapFromInterface.Namespace}.IMapFrom<{genericTypeName}>.Mapping";

                    var implementedMethod = type.GetMethod(implementedMethodName, BindingFlags.NonPublic | BindingFlags.Instance);

                    var methodInfo = implementedMethod ?? mapFromInterface.GetMethod("Mapping");

                    methodInfo?.Invoke(instance, new object[] { this });
                }
            }
        }

        private void ApplyMapToMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var mapToInterfaces = type.GetInterfaces().Where(i => i.Name == "IMapTo`1");

                foreach (var mapToInterface in mapToInterfaces)
                {
                    var genericType = mapToInterface.GenericTypeArguments[0];

                    var genericTypeName = $"{genericType.Namespace}.{genericType.Name}";

                    var implementedMethodName = $"{mapToInterface.Namespace}.IMapTo<{genericTypeName}>.Mapping";

                    var implementedMethod = type.GetMethod(implementedMethodName, BindingFlags.NonPublic | BindingFlags.Instance);

                    var methodInfo = implementedMethod ?? mapToInterface.GetMethod("Mapping");

                    methodInfo?.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}
