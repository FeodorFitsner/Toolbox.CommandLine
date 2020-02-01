using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class VerbTest
    {
        [TestMethod]
        public void ParseAddVerb()
        {
            var cut = Parser.Create<VerbAddOption, VerbListOption>();

            const string name = "some name";

            var args = new[] { "add", "-n", name };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.Succeeded, result.Result);
            Assert.IsInstanceOfType(result.Option, typeof(VerbAddOption));
            Assert.AreEqual(name, ((VerbAddOption)result.Option).Name);

        }

        [TestMethod]
        public void ParseListVerb()
        {
            var cut = Parser.Create<VerbAddOption, VerbListOption>();

            var args = new[] { "list", "-a" };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.Succeeded, result.Result);
            Assert.IsInstanceOfType(result.Option, typeof(VerbListOption));
            Assert.IsTrue(((VerbListOption)result.Option).Active);
        }

        [TestMethod]
        public void RequestHelpOnVerbs()
        {
            var cut = Parser.Create<VerbAddOption, VerbListOption>();

            var args = new string[] { "add", "-h" };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.RequestHelp, result.Result);
            Assert.IsInstanceOfType(result.Option, typeof(VerbAddOption));
        }

        [TestMethod]
        public void RequestHelp()
        {
            var cut = Parser.Create<VerbAddOption, VerbListOption>();

            var args = new string[] { "-h" };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.RequestHelp, result.Result);
            Assert.IsNull(result.Option);
        }
    }
}
