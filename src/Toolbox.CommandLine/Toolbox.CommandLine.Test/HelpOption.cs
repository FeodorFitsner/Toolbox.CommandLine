using System.ComponentModel;

namespace Toolbox.CommandLine.Test
{
    class HelpOption
    {
        [Option("text"), Position(0), Mandatory]
        public string Text { get; set; }

        [Option("number"), Position(1), DefaultValue(42)]
        public int Number { get; set; }

        [Option("date")]
        public int Date { get; set; }

        [Option("names"), DefaultValue(new string[1] { "John" })]
        public string[] Names { get; set; }

        [Option("quiet")]
        public bool Quiet { get; set; }

    }
}
