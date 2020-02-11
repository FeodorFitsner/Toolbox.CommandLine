using System;

namespace Toolbox.CommandLine.Test
{
    class BadPositionOptionDuplicate
    {
        [Option("text"), Position(1)]
        public string Text { get; set; }

        [Option("number"), Position(1)]
        public int Number { get; set; }

        [Option("date"), Position(2)]
        public DateTime Date { get; set; }
    }
}
