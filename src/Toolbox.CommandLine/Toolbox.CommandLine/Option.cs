using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine
{
    [DebuggerDisplay("{Name,nq} -> {Property.PropertyName,nq}")]
    public class Option
    {
        public Option(PropertyInfo property)
        {
            var optionAttribute = property.GetCustomAttribute<OptionAttribute>();

            Name = optionAttribute.Name;
            Property = property;
            DefaultValue = property.GetCustomAttribute<DefaultValueAttribute>();
            Mandatory = property.GetCustomAttribute<MandatoryAttribute>() != null;
        }

        public string Name { get; }
        public PropertyInfo Property { get; }
        public DefaultValueAttribute DefaultValue { get; }

        public bool Mandatory { get; }

        public bool IsSwitch => Property.PropertyType == typeof(bool);

        internal void SetValue(object option, object value)
        {
            if (!Property.PropertyType.IsAssignableFrom(value.GetType()))
            {
                if (Property.PropertyType.IsGenericType)
                {
                    var genericType = Property.PropertyType.GetGenericTypeDefinition();
                    if (genericType == typeof(Nullable<>))
                    {
                        value = Convert.ChangeType(value, Property.PropertyType.GetGenericArguments()[0]);
                    }
                }
                else
                {
                    value = Convert.ChangeType(value, Property.PropertyType);
                }
            }   
            

            Property.SetValue(option, value);
        }

        internal void SetDefault(object option)
        {
            if (DefaultValue != null)
                Property.SetValue(option, DefaultValue.Value);
        }
    }
}
