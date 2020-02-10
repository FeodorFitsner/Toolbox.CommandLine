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
                var noVerbs = Arguments.Where(a => a.Verb == null);
                if (noVerbs.Any())
                {
                    throw new ArgumentException($"no verb on types: {string.Join(", ", noVerbs.Select(a => a.Type.FullName))}");
                }

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
        public static Parser Create<T1,T2>() where T1 : new() where T2 : new()
        {
            return new Parser(typeof(T1), typeof(T2));
        }

        public Arguments[] Arguments { get; }
        public char OptionChar { get; set; } = '-';

        public List<string> HelpOptions { get; } = new List<string> { "?", "h", "help" };
        internal bool IsHelp(string arg)
        {
            if (arg == "") return false;
            if (arg[0] != OptionChar) return false;
            var name = arg.Substring(1);
            return HelpOptions.Contains(name);
        }

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
                    result.SetState(State.MissingVerb);
                }
                else
                {
                    var verb = queue.Dequeue();

                    if (verb=="")
                        result.SetState(State.MissingVerb, "empty verb");
                    else if (IsHelp(verb))
                    {
                        result.SetState(State.RequestHelp);
                    }
                    else
                    {

                        arguments = Arguments.FirstOrDefault(a => a.Verb == verb);
                        if (arguments == null)
                            result.SetState(State.MissingVerb, $"verb '{args[0]}' not defined");
                    }
                }                    
            }

            if (result.State== State.Succeeded)
            {
                arguments.Parse(result, queue);
            }
          
            return result;
        }
    }
}
