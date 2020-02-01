using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine.Test
{
    [Verb("add")]
    class VerbAddOption
    {
        [Option("n")]
        public string Name { get; set; }

    }
}
