using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

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
            Position = property.GetCustomAttribute<PositionAttribute>()?.Position;
        }

        public string Name { get; }
        public PropertyInfo Property { get; }
        public DefaultValueAttribute DefaultValue { get; }
        public int? Position { get; }

        public bool Mandatory { get; }

        public bool IsSwitch => Property.PropertyType == typeof(bool);

        internal void SetValue(object option, string value)
        {
            object optionValue;
            if (Property.PropertyType.IsArray)
            {
                var elementType = Property.PropertyType.GetElementType();
                var parts = value.Split(',');

                var array = Array.CreateInstance(elementType, parts.Length);
                for (var i = 0; i < parts.Length; i++)
                {
                    array.SetValue(ConvertTo(elementType, parts[i]), i);
                }
                optionValue = array;
            }
            else 
            {
                optionValue = ConvertTo(Property.PropertyType, value);
            }   
            
            Property.SetValue(option, optionValue);
        }

        private object ConvertTo(Type type, string value)
        {
            if (type.IsAssignableFrom(typeof(string))) return value;

            if (Property.PropertyType.IsGenericType)
            {
                var genericType = Property.PropertyType.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    return Convert.ChangeType(value, Property.PropertyType.GetGenericArguments()[0]);
                }
            }
            return Convert.ChangeType(value, Property.PropertyType);
        }

        internal void SetDefault(object option)
        {
            if (DefaultValue != null)
                Property.SetValue(option, DefaultValue.Value);
        }

        internal void SetSwitch(object option)
        {
            Property.SetValue(option, true);
        }
    }
}
