using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine
{
    public class Option
    {
        public Option(PropertyInfo property)
        {
            var optionAttribute = property.GetCustomAttribute<OptionAttribute>();

            Name = optionAttribute.Name;
            Property = property;
            DefaultValue = property.GetCustomAttribute<DefaultValueAttribute>();
        }

        public string Name { get; }
        public PropertyInfo Property { get; }
        public DefaultValueAttribute DefaultValue { get; }

        public bool IsSwitch => Property.PropertyType == typeof(bool);

        internal void SetValue(object option, object value)
        {
            Property.SetValue(option, value);
        }

        internal void SetDefault(object option)
        {
            if (DefaultValue != null)
                Property.SetValue(option, DefaultValue.Value);
        }
    }
}
