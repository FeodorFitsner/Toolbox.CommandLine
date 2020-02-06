using System;

namespace Toolbox.CommandLine
{
    /// <summary>
    /// Marks a property as mandatory
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class MandatoryAttribute : Attribute
    {
    }
}
