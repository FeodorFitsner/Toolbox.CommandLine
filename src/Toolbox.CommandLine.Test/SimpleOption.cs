using System.ComponentModel;

namespace Toolbox.CommandLine.Test
{
    class SimpleOption
    {
        public const string DefaultFileName = "default.txt";

        [Option("f"), DefaultValue(DefaultFileName)]
        public string FileName { get; set; }
    }
}
