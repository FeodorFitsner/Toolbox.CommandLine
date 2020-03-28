using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Toolbox.CommandLine
{
    /// <summary>
    /// Commandline parser that maps arguments from the command line to objects.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="Parser"/> class.
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

            if (types.Length == 1)
                OptionTypes = new Dictionary<string, OptionType> { { "", new OptionType(this, types[0], "") } };
            else
            { 
                var optionTypes = types.Select(t => new OptionType(this, t)).ToArray();
                var noVerbs = optionTypes.Where(ot => string.IsNullOrEmpty(ot.Verb));
                if (noVerbs.Any())
                {
                    throw new ArgumentException($"Empty verbs not allowed.", nameof(types));
                }
                if (optionTypes.GroupBy(ot => ot.Verb).Any(g => g.Count() > 1))
                {
                    throw new ArgumentException($"Duplicate verbs on parsing types.", nameof(types));
                }
                OptionTypes = optionTypes.ToDictionary(ot => ot.Verb);
            }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="verbs">A dictionary of verbs and options types.</param>
        /// <remarks>
        /// The verbs from the <see cref="VerbAttribute"/> are ignored.
        /// </remarks>
        public Parser(IDictionary<string, Type> verbs)
        {
            if (verbs.Count < 1)
                throw new ArgumentException($"Minimum of two verbs need to be specified.", nameof(verbs));
            if (verbs.ContainsKey(""))
                throw new ArgumentException($"Empty verbs not allowed.", nameof(verbs));

            OptionTypes = verbs.ToDictionary(kvp => kvp.Key, kvp => new OptionType(this, kvp.Value));
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
        public IReadOnlyDictionary<string,OptionType> OptionTypes { get; private set; }
        /// <summary>
        /// Gets the first <see cref="OptionType"/>.
        /// </summary>
        public OptionType OptionType => OptionTypes.First().Value;
        
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

            var result = new ParseResult { Parser = this };

            var optionType = OptionType;
            if (OptionTypes.Count > 1)
            {
                if (queue.Count==0)
                {
                    result.SetState(State.MissingVerb, "no verb given");
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
                        if (!OptionTypes.TryGetValue(verb, out optionType))
                            result.SetState(State.MissingVerb, $"verb '{args[0]}' not defined");

                        result.Verb = verb;
                    }
                }                    
            }

            if (result.State == State.Succeeded)
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
            var collector = new StringCollector(width);
            var assembly = Assembly.GetEntryAssembly();

            string name;
            string executable;
            string version;
            if (assembly != null)
            {
                version = assembly.GetName().Version.ToString();
                executable = Path.GetFileName(assembly.Location);

                var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
                if (titleAttribute != null)
                    name = titleAttribute.Title;
                else
                    name = Path.GetFileNameWithoutExtension(executable);
            }
            else
            {
                name = "<no assembly title>";
                version = "<no assembly version>";
                executable = "<no executable>";
            }
            collector.AppendLine($"{name} - {version}");
            collector.AppendLine("");

            var descriptionAttribute = assembly?.GetCustomAttribute<AssemblyDescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                collector.AppendLine("DESCRIPTION");
                collector.Indent = 2;
                collector.AppendLine(descriptionAttribute.Description);
                collector.Indent = 0;
                collector.AppendLine("");
            }

            if (verb != null)
            {
                OptionTypes[verb].GetHelpText(collector, executable);
            }
            else if (OptionTypes.Count == 1)
                OptionType.GetHelpText(collector, executable);
            else
            {
                collector.AppendLine("SYNTAX");
                collector.Indent = 2;
                collector.AppendLine($"{executable} <verb> <options>");
                collector.AppendLine("");
                collector.Indent = 0;

                collector.AppendLine("VERBS");
                collector.Indent = 2;
                foreach (var optionType in OptionTypes.Values)
                {
                    collector.Append(optionType.Verb);
                    if (optionType.Description != "")
                        collector.AppendLine($"- {optionType.Description}");
                    else
                        collector.AppendLine("");
                }
                collector.Indent = 0;
            }

            return collector.ToString();
        }
    }
}