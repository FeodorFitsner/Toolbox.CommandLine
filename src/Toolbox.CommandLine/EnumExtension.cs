using System;
using System.Linq;

namespace Toolbox.CommandLine
{
    internal static class EnumExtension
    {
        public static bool IsOneOf<T>(this T instance, params T[] values) where T : Enum
        {
            return values.Contains(instance);
        }
        public static bool IsNoneOf<T>(this T instance, params T[] values) where T : Enum
        {
            return !values.Contains(instance);
        }
    }
}
