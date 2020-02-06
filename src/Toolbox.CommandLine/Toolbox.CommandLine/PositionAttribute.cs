using System;

namespace Toolbox.CommandLine
{
    /// <summary>
    /// Marks the postion of an option in the argument list.
    /// </summary>
    /// <see cref="OptionAttribute"/>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class PositionAttribute : Attribute
    {
        public PositionAttribute(int position)
        {
            if (position < 0)
                throw new ArgumentException("negative postion", nameof(position));

            Position = position;
        }

        public int Position { get;  }
    }
}
