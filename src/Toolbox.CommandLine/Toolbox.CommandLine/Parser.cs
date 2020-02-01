using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine
{
    /// <summary>
    /// Commandline parser that maps arguments from the command line to objects.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Initialize a new <see cref="Parser"/> instance.
        /// </summary>
        public Parser(params Type[] types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));
            if (types.Length == 0)
                throw new ArgumentException("No types specified.", nameof(types));

            Arguments = types.Select(t => new Arguments(this, t)).ToArray();
            if (Arguments.Length > 1)
            {
                if (Arguments.GroupBy(a => a.Verb).Any(g => g.Count()>1))
                {
                    throw new ArgumentException($"duplicate verbs on parsing types.", nameof(types));
                }
            }
        }

        public static Parser Create<T>() where T : new()
        {
            return new Parser(typeof(T));
        }

        public Arguments[] Arguments { get; }
        public char OptionChar { get; set; } = '-';

        public ParseResult Parse(params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var queue = new Queue<string>(args);

            var result = new ParseResult();

            var arguments = Arguments[0];
            if (Arguments.Length>1)
            {
                if (queue.Count==0)
                {
                    result.SetResult(Result.MissingVerb);
                }
                else
                {
                    var verb = queue.Dequeue();

                    arguments = Arguments.FirstOrDefault(a => a.Verb == verb);
                    if (arguments == null)
                        result.SetResult(Result.MissingVerb, $"verb '{args[0]}' not defined");                    
                }                    
            }

            if (result.Result == Result.Succeeded)
            {
                arguments.Parse(result, queue);
            }
          
            return result;
        }
    }
}
