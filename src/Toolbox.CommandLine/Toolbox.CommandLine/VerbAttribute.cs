using System;

namespace Toolbox.CommandLine
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class VerbAttribute : Attribute
    {
        public VerbAttribute(string verb)
        {
            Verb = verb;
        }

        public string Verb { get; }
    }    
}
