using System.ComponentModel;

namespace Toolbox.CommandLine.Demo1
{
    class DemoOptions
    {
        [Option("name"), Mandatory, Position(0)]
        [Description("The name of a person.")]
        public string Name { get; set; }

        [Option("company"), Position(1), DefaultValue("myCompany")]
        [Description("The company of a person.")]
        public string Company { get; set; }

        [Option("number")]
        [Description("Some number")]
        public int? Number { get; set; }

        [Option("quiet")]
        [Description("Turns off most of the output")]
        public bool Quiet { get; set; }
    }
}
