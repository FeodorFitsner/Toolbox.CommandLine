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
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionAttribute"/>
        /// </summary>
        /// <param name="position">Index of the option (starts at <c>0</c>)</param>
        public PositionAttribute(int position)
        {
            if (position < 0)
                throw new ArgumentException("negative postion", nameof(position));

            Position = position;
        }

        /// <summary>
        /// Gets the position
        /// </summary>
        public int Position { get;  }
    }
}
