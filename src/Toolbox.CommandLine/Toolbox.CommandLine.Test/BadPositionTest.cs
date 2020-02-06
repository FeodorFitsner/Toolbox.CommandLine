using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class BadPositionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsOnDuplicatePosition()
        {
            Parser.Create<BadPositionOptionDuplicate>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsOnNegativePosition()
        {
            Parser.Create<BadPositionOptionNegative>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsOnMissingPosition()
        {
            Parser.Create<BadPositionOptionMissing>();
        }
    }
}
