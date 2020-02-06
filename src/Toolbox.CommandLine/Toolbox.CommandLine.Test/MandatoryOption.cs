namespace Toolbox.CommandLine.Test
{
    class MandatoryOption
    {
        [Option("f"), Mandatory]
        public string FileName { get; set; }

        [Option("t"), Mandatory]
        public string Text { get; set; }

    }
}
