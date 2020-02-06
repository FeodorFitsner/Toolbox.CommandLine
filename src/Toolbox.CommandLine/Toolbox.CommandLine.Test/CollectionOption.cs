using System.ComponentModel;

namespace Toolbox.CommandLine.Test
{
    class CollectionOption
    {
        [Option("names"), DefaultValue(new string[1] { "John" })]
        public string[] Names { get; set; }
    }
}
