using System;
using System.Collections.Generic;

namespace GlobalQueryFilters.Magic
{
    public static class TypeExtensions
    {
        public static List<Type> GetAllBaseTypes(this Type type)
        {
            var currentType = type;
            var results = new List<Type>();
            while (currentType.BaseType != null)
            {
                results.Add(currentType.BaseType);
                currentType = currentType.BaseType;
            }

            return results;
        }
    }
}