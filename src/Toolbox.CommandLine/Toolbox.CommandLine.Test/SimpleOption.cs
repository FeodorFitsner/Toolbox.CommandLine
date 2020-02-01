using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine.Test
{
    class SimpleOption
    {
        public const string DefaultFileName = "default.txt";

        [Option("f"), DefaultValue(DefaultFileName)]
        public string FileName { get; set; }
    }
}
