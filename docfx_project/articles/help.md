# Help

As a standard the parser reacts to argument `-?`, `-h` or '-help' with a requst for help.
The result will indicate this by the value `State.RequestHelp`.

It is the caller that must present the help in his favorite way.
The call can inspect the `Parser.OptionType` or `Parser.OptionTypes` to create a help on his own.
Or use the `GetHelpText`-Methods to create a text representation.

```
    // given two options with verb 'add' and 'remove'
    var parser = Parser.Create<DemoAddOptions, DemoRemoveOptions>();
    var result = parser.Parse(args)
        .OnError(r =>
        {
            Console.WriteLine(r.Text);
            return -2;
        })
        .OnHelp(r =>
        {
            // just output the standard help
            Console.WriteLine(parser.GetHelpText(r.Verb));
            return -1;
        })
        .On<DemoAddOptions>(o =>
        {
            Console.WriteLine($"Options [{o.GetType().Name}]");
            Console.WriteLine($"Name = '{o.Name}'");
            Console.WriteLine($"Company = '{o.Company}'");
            Console.WriteLine($"Quiet = '{o.Quiet}'");
            return 0;
        })
        .On<DemoRemoveOptions>(o =>
        {
            Console.WriteLine($"Options [{o.GetType().Name}]");
            Console.WriteLine($"Name = '{o.Name}'");
            Console.WriteLine($"Quiet = '{o.Quiet}'");
            return 0;
        });

```

```
some.exe -?
```
will output an help for the verbs 'add' and 'remove'.

```
some.exe add -?
```
will output an detailed help for the verb 'add'.

If only one option type is given then the detailed help for the type is given.

## Supressing or changing 

If the help options are not wanted or must be changed, simply set `Parser.HelpOptions` to some other value.
For no help use an empty array.
```
parser.HelpOption = new [] { "X" };
```
if you like an big X as the option for the help request.