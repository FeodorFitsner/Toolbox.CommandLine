﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class SimpleOptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowOnConstrutorNoTypes()
        {
            new Parser();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowOnConstrutorEmptyTypes()
        {
            new Parser(Type.EmptyTypes);            
        }

        [TestMethod]
        public void CompleteConstrutorSimpleOption()
        {
            var cut = new Parser(typeof(SimpleOption));

            Assert.AreEqual(1, cut.Arguments.Length);
            Assert.AreEqual(typeof(SimpleOption), cut.Arguments[0].Type);
            Assert.IsNull(cut.Arguments[0].Verb);
            Assert.AreEqual(1, cut.Arguments[0].Options.Length);
            Assert.AreEqual("f", cut.Arguments[0].Options[0].Name);
            Assert.AreEqual("FileName", cut.Arguments[0].Options[0].Property.Name);
        }

        [TestMethod]
        public void ParseSimpleOption()
        {
            var cut = Parser.Create<SimpleOption>();

            const string filename = "myText.txt";

            var args = new[] { "-f", filename };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.Succeeded, result.Result);
            Assert.IsInstanceOfType(result.Option, typeof(SimpleOption));
            Assert.AreEqual(filename, ((SimpleOption)result.Option).FileName);
        }

        [TestMethod]
        public void DuplicateSimpleOption()
        {
            var cut = Parser.Create<SimpleOption>();

            const string filename = "myText.txt";

            var args = new[] 
            { 
                "-f", filename,
                "-f", filename
            };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.DuplicateOption, result.Result);
            Assert.IsInstanceOfType(result.Option, typeof(SimpleOption));            
        }

        [TestMethod]
        public void ParseEmptyArgs()
        {
            var cut = Parser.Create<SimpleOption>();

            var args = new string[0];

            var result = cut.Parse(args);

            Assert.AreEqual(Result.Succeeded, result.Result);
            Assert.IsInstanceOfType(result.Option, typeof(SimpleOption));
            Assert.AreEqual(SimpleOption.DefaultFileName, ((SimpleOption)result.Option).FileName);
        }

        [TestMethod]
        public void RequestHelpQuestionMark()
        {
            var cut = Parser.Create<SimpleOption>();

            var args = new string[] { "-?" };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.RequestHelp, result.Result);
        }

        [TestMethod]
        public void RequestHelpByH()
        {
            var cut = Parser.Create<SimpleOption>();

            var args = new string[] { "-h" };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.RequestHelp, result.Result);
        }

        [TestMethod]
        public void RequestHelpByHelp()
        {
            var cut = Parser.Create<SimpleOption>();

            var args = new string[] { "-help" };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.RequestHelp, result.Result);
        }

        [TestMethod]
        public void RequestHelpNoHelp()
        {
            var cut = Parser.Create<SimpleOption>();
            cut.HelpOptions.Clear();

            var args = new string[] { "-?" };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.UnknownOption, result.Result);
        }
    }
}