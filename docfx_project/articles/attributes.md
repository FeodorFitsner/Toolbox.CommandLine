# Attributes

These are all the attributes and there usages in this component.

## <xref:Toolbox.CommandLine.OptionAttribute>

```
OptionAttribute(string name)
```

This attribute declares a property to be usable as an argument on the command line.

## <xref:Toolbox.CommandLine.PositionAttribute>
```
PositionAttribute(int position)
```

This declares that an argument can be used at the given postion (starting at `0`) on the command line.
The name of the option must not be give.

All given postions must start at `0` and must increase always by `1`.
```
    class DemoOptions
    {
        [Option("name"), Position(0)]
        public string Name { get; set; }

        [Option("company"), Position(1)]
        public string Company { get; set; }

        [Option("quiet")]
        public bool Quiet { get; set; }
    }
```
This can be used like
```
some.exe Hugo "Big Tech Corp." -quiet
```
or
```
some.exe Hugo -quiet - company "Big Tech Corp."
```
or just
```
some.exe Hugo
```

## <xref:Toolbox.CommandLine.MandatoryAttribute>
```
MandatoryAttribute()
```
This makes an option mandatory. If its argument is not given on the command line, the parsing fails.

## <xref:Toolbox.CommandLine.VerbAttribute>
```
VerbAttribute(string name)
```
This provides a verb for an option type, when used with mutiple options.

## DescriptionAttribute
```
DescriptionAttribute(string text)
```
This provides a description for an option or option type. This will be used for the help options.

## DefaultValueAttribute
```
DefaultValueOption(object value)
```
This provides a default value for an option. It this option is not give on the command line the property will
be set to `value`.