using System;

namespace Toolbox.CommandLine.Test
{
    class BadPositionOptionMissing
    {
        [Option("text"), Position(1)]
        public string Text { get; set; }

        [Option("number"), Position(0)]
        public int Number { get; set; }

        [Option("date"), Position(4)]
        public DateTime Date { get; set; }
    }
}
