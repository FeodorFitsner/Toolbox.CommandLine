using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class FluentResultTest
    {
        [TestMethod]
        public void ExecuteFirstVerb()
        {
            var parser = Parser.Create<VerbAddOption, VerbListOption>();

            const string name = "some name";

            var args = new[] { "add", "-n", name };

            var cut = parser.Parse(args);

            var result = cut
                            .On<VerbAddOption>(o => 1)
                            .On<VerbListOption>(o => 2)
                            .OnHelp(r => -1)
                            .OnError(r => -2).Return;

            Assert.AreEqual(1, result, "On<VerbAddOption> not executed");
        }

        [TestMethod]
        public void ExecuteSecondVerb()
        {
            var parser = Parser.Create<VerbAddOption, VerbListOption>();

            var args = new[] { "list", "-a" };

            var cut = parser.Parse(args);

            var result = cut
                            .On<VerbAddOption>(o => 1)
                            .On<VerbListOption>(o => 2)
                            .OnHelp(r => -1)
                            .OnError(r => -2).Return;

            Assert.AreEqual(2, result, "On<VerbListOption> not executed");
        }

        [TestMethod]
        public void ExecuteOnError()
        {
            var parser = Parser.Create<VerbAddOption, VerbListOption>();

            var args = new[] { "list", "-g" };

            var cut = parser.Parse(args);

            var result = cut
                            .On<VerbAddOption>(o => 1)
                            .On<VerbListOption>(o => 2)
                            .OnHelp(r => -1)
                            .OnError(r => -2).Return;

            Assert.AreEqual(-2, result, "OnError not executed");
        }

        [TestMethod]
        public void ExecuteHelpWithoutVerb()
        {
            var parser = Parser.Create<VerbAddOption, VerbListOption>();

            var args = new[] { "-?" };

            var cut = parser.Parse(args);

            var result = cut
                            .On<VerbAddOption>(o => 1)
                            .On<VerbListOption>(o => 2)
                            .OnHelp(r => r.Option==null ? -1 : -3)
                            .OnError(r => -2).Return;

            Assert.AreEqual(-1, result, "OnHelp executed incorrect");
        }

        [TestMethod]
        public void ExecuteHelpWithFirstVerb()
        {
            var parser = Parser.Create<VerbAddOption, VerbListOption>();

            var args = new[] { "add", "-?" };

            var cut = parser.Parse(args);

            var result = cut
                            .On<VerbAddOption>(o => 1)
                            .On<VerbListOption>(o => 2)
                            .OnHelp(r => r.Option != null && r.Option is VerbAddOption ? -3 : -1)
                            .OnError(r => -2).Return;

            Assert.AreEqual(-3, result, "OnHelp executed incorrect");
        }

        [TestMethod]
        public void ParseVerbsSameOptionClass()
        {
            var verbs = new Dictionary<string, Type>
            {
                { "add", typeof(VerbAddOption)  },
                { "remove", typeof(VerbAddOption)  },
            };
            var parser = new Parser(verbs);

            const string name = "some name";

            var args = new[] { "add", "-n", name };

            var cut = parser.Parse(args);

            var result = cut
                    .On<VerbAddOption>("add", o => 1)
                    .On<VerbAddOption>("remove", o => 2)
                    .OnHelp(r => r.Option != null && r.Option is VerbAddOption ? -3 : -1)
                    .OnError(r => -2).Return;


            Assert.AreEqual(1, result, "On<VerbAddOption> not executed");
        }

    }
}
