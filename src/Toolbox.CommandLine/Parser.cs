using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="types">The types of the option objects.</param>
        /// <remarks>
        /// If more than one type is given, all muss implement verbs (<see cref="VerbAttribute"/>). 
        /// All types must have a default constructor.
        /// </remarks>
        public Parser(params Type[] types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));
            if (types.Length == 0)
                throw new ArgumentException("No types specified.", nameof(types));

            OptionTypes = types.Select(t => new OptionType(this, t)).ToArray();
            if (OptionTypes.Length > 1)
            {
                var noVerbs = OptionTypes.Where(a => a.Verb == null);
                if (noVerbs.Any())
                {
                    throw new ArgumentException($"no verb on types: {string.Join(", ", noVerbs.Select(a => a.Type.FullName))}");
                }

                if (OptionTypes.GroupBy(a => a.Verb).Any(g => g.Count()>1))
                {
                    throw new ArgumentException($"duplicate verbs on parsing types.", nameof(types));
                }
            }
        }

        /// <summary>
        /// Create a <see cref="Parser"/> for type T.
        /// </summary>
        /// <typeparam name="T">A type containing the options</typeparam>
        /// <returns>Initialized parser</returns>
        public static Parser Create<T>() where T : new()
        {
            return new Parser(typeof(T));
        }

        /// <summary>
        /// Create a <see cref="Parser"/> for type T1 and T2.
        /// </summary>
        /// <typeparam name="T1">A type containing the options</typeparam>
        /// <typeparam name="T2">A type containing the options</typeparam>
        /// <returns>Initialized parser</returns>
        /// <remarks>Both types must implement verbs.</remarks>
        /// <see cref="VerbAttribute"/>
        public static Parser Create<T1,T2>() where T1 : new() where T2 : new()
        {
            return new Parser(typeof(T1), typeof(T2));
        }

        /// <summary>
        /// Get the list of possible options.
        /// </summary>
        public OptionType[] OptionTypes { get; }
        /// <summary>
        /// Gets or sets the character begining an option.
        /// </summary>
        /// <remarks>
        /// The default is <c>'-'</c>.
        /// </remarks>
        public char OptionChar { get; set; } = '-';

        /// <summary>
        /// Gets the list of options that trigger the help request.
        /// </summary>
        /// <remarks>
        /// The default is <c>?</c>, <c>h</c> and <c>help</c>.
        /// The list can be modified to meet your needs.
        /// The options take precedence over the options defined by the <see cref="OptionTypes"/>.
        /// </remarks>
        /// <see cref="State.RequestHelp"/>
        public List<string> HelpOptions { get; } = new List<string> { "?", "h", "help" };

        internal bool IsHelp(string arg)
        {
            if (arg == "") return false;
            if (arg[0] != OptionChar) return false;
            var name = arg.Substring(1);
            return HelpOptions.Contains(name);
        }

        /// <summary>
        /// Parses the given arguments into a filled option object.
        /// </summary>
        /// <param name="args">the arguments to parse</param>
        /// <returns>The parsed result</returns>
        /// <see cref="ParseResult"/>
        public ParseResult Parse(params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var queue = new Queue<string>(args);

            var result = new ParseResult();

            var optionType = OptionTypes[0];
            if (OptionTypes.Length>1)
            {
                if (queue.Count==0)
                {
                    result.SetState(State.MissingVerb);
                }
                else
                {
                    var verb = queue.Dequeue();

                    if (verb == "")
                    {
                        result.SetState(State.MissingVerb, "empty verb");
                    }
                    else if (IsHelp(verb))
                    {
                        result.SetState(State.RequestHelp);
                    }
                    else
                    {
                        optionType = OptionTypes.FirstOrDefault(a => a.Verb == verb);
                        if (optionType == null)
                            result.SetState(State.MissingVerb, $"verb '{args[0]}' not defined");

                        result.Verb = verb;
                    }
                }                    
            }

            if (result.State== State.Succeeded)
            {
                optionType.Parse(result, queue);
            }
          
            return result;
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <param name="verb">creates the help text for this option.</param>
        /// <param name="width">Width of the genreated text. (default: 80)</param>
        /// <returns>The help text</returns>
        /// <remarks>
        /// The returned text is not complete. Work in progress.
        /// </remarks>
        public string GetHelpText(string verb = null, int width = 80)
        {
            if (OptionTypes.Length == 1 && verb != null)
                throw new ArgumentException("no verb allowed on single option", nameof(verb));
            if (OptionTypes.Length > 1 && verb == null)
                throw new ArgumentNullException("verb mandatory on multiple options", nameof(verb));

            var optionType = OptionTypes[0];
            if (verb != null)
            {
                optionType = OptionTypes.FirstOrDefault(a => a.Verb == verb);
                if (optionType == null)
                    throw new ArgumentOutOfRangeException("verb '{verb}' not found", nameof(verb));
            }

            return optionType.GetHelpText(width);
        }
    }
}