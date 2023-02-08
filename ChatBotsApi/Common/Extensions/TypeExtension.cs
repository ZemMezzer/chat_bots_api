using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBotsApi.Common.Extensions
{
    internal static class TypeExtension
    {
        public static Type[] GetAllConcreteChildTypes(this Type type)
        {
            List<Type> types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                types.AddRange(assembly.GetTypes().Where(t => (type.IsInterface ? t.GetInterfaces().Contains(type) : t.IsSubclassOf(type)) && t.IsClass && !t.IsAbstract));
            

            return types.ToArray();
        }

        public static IEnumerable<T> ActivateAllTypes<T>(this Type[] types)
        {
            foreach (var type in types)
                yield return (T)Activator.CreateInstance(type);
        }
    }
}