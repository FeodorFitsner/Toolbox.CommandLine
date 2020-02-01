using System;

namespace Toolbox.CommandLine
{
    /// <summary>
    /// Attribute to map a command line argument to a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class OptionAttribute : Attribute
    {
        public OptionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
