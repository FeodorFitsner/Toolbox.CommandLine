using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine.Test
{
    [Verb("list")]
    class VerbListOption
    {
        [Option("a")]
        public bool Active { get; set; }
    }
}
