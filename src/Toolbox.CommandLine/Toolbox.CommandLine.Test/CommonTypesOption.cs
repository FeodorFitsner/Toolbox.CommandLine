using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine.Test
{
    class CommonTypesOption
    {
        [Option("text")]
        public string Text { get; set; }

        [Option("switch")]
        public bool Switch { get; set; }

        [Option("number")]
        public int Number { get; set; }

        [Option("decimal")]
        public decimal Decimal { get; set; }

        [Option("date")]
        public DateTime Date { get; set; }

        [Option("now")]
        public DateTime Now { get; set; }

    }
}
