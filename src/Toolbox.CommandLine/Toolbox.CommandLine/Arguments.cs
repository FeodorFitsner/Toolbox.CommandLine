using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Toolbox.CommandLine
{
    public class Arguments
    {
        public Arguments(Parser parser, Type type)
        {
            Parser = parser;
            Type = type;
            Verb = type.GetCustomAttribute<VerbAttribute>()?.Verb;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                            .Where(p => p.GetCustomAttribute<OptionAttribute>() != null && p.CanWrite);

            Options = properties.Select(p => new Option(p)).ToArray();
            if (Options.GroupBy(o => o.Name).Any(g => g.Count() > 1))
            {
                throw new ArgumentException($"Duplicate options on type {type.FullName}.", nameof(type));
            }

            PositionOptions = Options.Where(o => o.Position.HasValue).OrderBy(o => o.Position).ToArray();
            if (PositionOptions.Any(p => p.Position != Array.IndexOf(PositionOptions, p)))
            {
                throw new ArgumentException($"Bad positions for options on type {type.FullName}.", nameof(type));
            }
        }

        public Parser Parser { get; }
        public Type Type { get; }
        public string Verb { get; }
        public Option[] Options { get; }
        public Option[] PositionOptions { get; }

        internal void Parse(ParseResult result, Queue<string> queue)
        {
            var counts = Options.ToDictionary(o => o.Name, o => 0);

            result.Option = Activator.CreateInstance(Type);
            Options.ToList().ForEach(o => o.SetDefault(result.Option));

            var count = 0;
            while (queue.Count>0)
            {
                var arg = queue.Dequeue();
                
                if (arg == "")
                    result.SetResult(Result.MissingOption, $"option exspected at [{count}].");
                else if (arg[0]!=Parser.OptionChar && count>=PositionOptions.Length)
                    result.SetResult(Result.MissingOption, $"option exspected at [{count}] '{arg}'.");
                else
                {
                    if (Parser.IsHelp(arg))
                    {
                        result.SetResult(Result.RequestHelp);
                    }
                    else
                    {
                        Option option;
                        string name;
                        string value = null;

                        if (arg[0] != Parser.OptionChar)
                        {
                            option = PositionOptions[count];
                            name = option.Name;
                            value = arg;
                        }
                        else
                        {
                            name = arg.Substring(1);
                            option = Options.FirstOrDefault(o => o.Name == name);
                        }

                        if (option == null)
                            result.SetResult(Result.UnknownOption, $"option not known at [{count}] '{arg}'.");
                        else
                        {
                            counts[option.Name]++;

                            if (option.IsSwitch)
                            {
                                option.SetSwitch(result.Option);
                            }
                            else
                            {
                                if (queue.Count == 0)
                                    result.SetResult(Result.MissingValue, $"option needs value after [{count}] '{arg}'.");
                                else
                                {
                                    if (value == null)
                                        value = queue.Dequeue();

                                    try
                                    {
                                        option.SetValue(result.Option, value);
                                    }
                                    catch (Exception exception)
                                    {
                                        result.SetResult(Result.BadValue, $"bad option value at [{count}] '{arg}'='{value}' ({exception.Message}).");
                                    }
                                }
                            }
                        }
                    }
                }
                count++;
            }

            if (result.Result == Result.Succeeded)
            {
                var mandatory = Options.Where(o => o.Mandatory && counts[o.Name] == 0);
                if (mandatory.Any())
                {
                    result.SetResult(Result.MandatoryOption, $"mandatory options: {string.Join(", ", mandatory.Select(o => o.Name))}");
                }
                else
                {
                    var duplicate = Options.Where(o => counts[o.Name]>1);
                    if (duplicate.Any())
                    {
                        result.SetResult(Result.DuplicateOption, $"duplicate options: {string.Join(", ", duplicate.Select(o => o.Name))}");
                    }
                }
            }
        }
    }
}
