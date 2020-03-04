# Array

As an option type also arrays can be used.

Just declare the property as an array.

```
    class DemoOptions
    {
        [Option("names")]
        public string[] Names { get; set; }
    }
```

Now the names can be give on the command line like
```
some.exe -names "Alice,Bob,Charlie"
```

The value of `name` is split at `,` and converted into an array. 
