using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine.Test
{
    class DuplicateOption
    {
        [Option("h")]
        public string SomeText { get; set; }
        [Option("h")]
        public string OtherText { get; set; }
    }
}
