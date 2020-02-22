using System;

namespace Toolbox.CommandLine
{
    /// <summary>
    /// Attribute to map a command line argument to a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class OptionAttribute : Attribute
    {
        /// <summary>
        /// Initialilizes a new instance of the <see cref="OptionAttribute"/> class.
        /// </summary>
        /// <param name="name"></param>
        public OptionAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of the option
        /// </summary>
        public string Name { get; }
    }
}
