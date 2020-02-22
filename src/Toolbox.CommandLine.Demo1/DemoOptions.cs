namespace Toolbox.CommandLine.Demo1
{
    class DemoOptions
    {
        [Option("name"), Mandatory, Position(0)]
        public string Name { get; set; }
    }
}
