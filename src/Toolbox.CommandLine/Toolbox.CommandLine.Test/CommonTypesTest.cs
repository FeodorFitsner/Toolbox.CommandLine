using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class CommonTypesTest
    {
        [TestMethod]
        public void ParseTypes()
        {
            var cut = Parser.Create<CommonTypesOption>();

            const string text = "hello";
            const int number = 42;
            const decimal dec = 47.11M;
            var date = DateTime.Today;
            var now = DateTime.Today.AddHours(15).AddMinutes(14).AddSeconds(53);
            
            var args = new string[]
            {
                "-text", text,
                "-switch",
                "-number", number.ToString(),
                "-decimal", dec.ToString(),
                "-date", date.ToString("d"),
                "-now", now.ToString("G")
            };

            var result = cut.Parse(args);

            Assert.AreEqual(Result.Succeeded, result.Result);
            Assert.IsInstanceOfType(result.Option, typeof(CommonTypesOption));

            var option = (CommonTypesOption)result.Option;

            Assert.AreEqual(text, option.Text);
            Assert.IsTrue(option.Switch);
            Assert.AreEqual(number, option.Number);
            Assert.AreEqual(dec, option.Decimal);
            Assert.AreEqual(date, option.Date);
            Assert.AreEqual(now, option.Now);
        }
    }
}
