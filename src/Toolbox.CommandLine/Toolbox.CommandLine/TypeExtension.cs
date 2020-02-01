using System;
using System.Linq;

namespace Toolbox.CommandLine
{
    internal static class TypeExtension
    {
        public static T GetCustomAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return type.GetCustomAttributes(type, inherit).Cast<T>().FirstOrDefault();
        }
    }
}
