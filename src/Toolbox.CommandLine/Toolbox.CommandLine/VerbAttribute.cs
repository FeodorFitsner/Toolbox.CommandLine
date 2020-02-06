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
        public VerbAttribute(string verb)
        {
            Verb = verb;
        }

        public string Verb { get; }
    }    
}
