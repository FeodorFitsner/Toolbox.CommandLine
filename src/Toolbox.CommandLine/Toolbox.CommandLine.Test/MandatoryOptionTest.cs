using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class MandatoryOptionTest
    {
        [TestMethod]
        public void ObmitAll()
        {
            var cut = Parser.Create<MandatoryOption>();

            var args = new string[0];

            var result = cut.Parse(args);

            Assert.AreEqual(Result.MandatoryOption, result.Result);
            Assert.IsInstanceOfType(result.Option, typeof(MandatoryOption));

            var option = (MandatoryOption)result.Option;

            Assert.IsNull(option.FileName);
            Assert.IsNull(option.Text);
        }
    }
}
