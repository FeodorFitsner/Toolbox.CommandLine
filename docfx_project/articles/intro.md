# Introduction

With this component it is possible to easily parse the command line arguments in to an object.

## Option class

The parsed options go into an object of some class, that needs to
* an default constructor
* some properties decorated with the <xref:Toolbox.CommandLine.OptionAttribute>

Some other attribute can provide futher information how the options are indent to be used.

A simple example of an option class.
```
    class DemoOptions
    {
        [Option("name")]
        public string Name { get; set; }

        [Option("company")]
        public string Company { get; set; }

        [Option("quiet")]
        public bool Quiet { get; set; }
    }
```

The argument of the `OptionAttribute` is the name the argument on the command line.
It can differ from the name of the propery.

Properties of most basic types are supported (i.e. `string`, `int`, `bool`, `DateTime`, `TimeSpan`).

Nullable types (i.e. `int?`) are supported too. If the argument is not given then the property will be `null`. 
In the case of just `int` the value would be `0`. So with these types it can be detected if an argument was given 
on the command line.

The example can be used on the command line like this
```
some.exe -name Hugo -company "Big Tech Corp." -quiet
```

The argument `quiet` does not take a value, since it automatically will set the property `Quiet` to `true`.
For all other arguments the value is converted to the corresponding type.

## Using the options

The easiest way to use these options is this example.
```
    static int Main(string[] args)
    {
        var parser = Parser.Create<DemoOptions>();
        var result = parser.Parse(args)
            .OnError(r =>
            {
                // gets called in case of an error in the arguments given
                Console.WriteLine(r.Text);
                return -2;
            })
            .On<DemoOptions>(o => 
            {
                // gets called when the arguments are succesfully parsed
                Console.WriteLine($"Options [{o.GetType().Name}]");
                Console.WriteLine($"Name = '{o.Name}'");
                Console.WriteLine($"Company = '{o.Company}'");
                Console.WriteLine($"Quiet = '{o.Quiet}'");
                return 0;
            });

        Console.WriteLine("");
        Console.WriteLine($"return = {result.Return}");

        return result.Return;
    }
```

You can - of course - just call `Parse` and interpret the `result` as you like.
