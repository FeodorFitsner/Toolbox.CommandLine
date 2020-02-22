using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine.Demo1
{
    class Program
    {
        static int Main(string[] args)
        {
            var parser = Parser.Create<DemoOptions>();
            var result = parser.Parse(args)
                .OnError(r =>
                {
                    Console.WriteLine(r.Text);
                    return -2;
                })
                .OnHelp(r => 
                {
                    Console.WriteLine(parser.GetHelpText());
                    return -1;
                })
                .On<DemoOptions>(o => 
                {
                    Console.WriteLine($"Name = '{o.Name}'");
                    return 0;
                }
                );

            Console.WriteLine($"Return = {result.Return}");

            return result.Return;
        }
    }
}
