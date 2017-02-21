using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers
{
    public static class TypeHelper
    {
        /// <summary>
        /// Gets all classes implementing given interface type, excluding the interface itself
        /// </summary>
        /// <param name="interfaceType">Type of the interface</param>
        /// <returns>Classes implementing given interface</returns>
        public static IList<Type> GetClassesImplementingInterface(Type interfaceType)
        {
            var entityTypes = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                entityTypes.AddRange(assembly.GetTypes().Where(p => interfaceType.IsAssignableFrom(p) && !p.IsInterface));
            }

            return entityTypes;
        }
    }
}
