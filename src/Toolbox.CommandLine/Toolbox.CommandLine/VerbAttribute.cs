using System;

namespace Toolbox.CommandLine
{
    /// <summary>
    /// Marks a class as options for a verb.
    /// </summary>
    /// <see cref="OptionAttribute"/>"/>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class VerbAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerbAttribute"/> class.
        /// </summary>
        /// <param name="verb"></param>
        public VerbAttribute(string verb)
        {
            Verb = verb;
        }

        /// <summary>
        /// Gets the verb
        /// </summary>
        public string Verb { get; }
    }    
}
