using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class HelpTest
    {
        [TestMethod]
        public void RequestHelp()
        {
            var cut = Parser.Create<HelpOption>();

            var args = new[] { "-?" };

            var result = cut.Parse(args);

            string text = null;

            var rc = result.OnHelp(r =>
            {
                text = cut.GetHelpText();
                return 5;
            }).Return;

            Assert.AreEqual(5, rc);            
        }

        [TestMethod]
        public void RequestHelpCommon()
        {
            var cut = Parser.Create<CommonTypesOption>();

            var args = new[] { "-?" };

            var result = cut.Parse(args);

            string text = null;

            var rc = result.OnHelp(r =>
            {
                text = cut.GetHelpText();
                return 5;
            }).Return;

            Assert.AreEqual(5, rc);
        }
    }
}
