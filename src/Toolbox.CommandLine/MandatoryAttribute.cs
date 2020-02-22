using System;

namespace Toolbox.CommandLine
{
    /// <summary>
    /// Marks a property of the options as mandatory
    /// </summary>
    /// <seealso cref="OptionAttribute"/>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class MandatoryAttribute : Attribute
    {
    }
}
