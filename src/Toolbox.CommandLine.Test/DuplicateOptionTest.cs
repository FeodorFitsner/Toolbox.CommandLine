using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class DuplicateOptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowOnDuplicateOption()
        {
            new Parser(typeof(DuplicateOption));

            Assert.Fail("no exception");
        }
    }
}
