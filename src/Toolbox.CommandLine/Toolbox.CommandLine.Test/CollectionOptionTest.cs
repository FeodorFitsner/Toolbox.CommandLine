using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Toolbox.CommandLine.Test
{
    [TestClass]
    public class CollectionOptionTest
    {
        [TestMethod]
        public void SimpleArray()
        {
            var cut = Parser.Create<CollectionOption>();

            var names = new[]
            {
                "Peter",
                "Paul",
                "Mary"
            };

            var args = new string[]
            {
                "-names", string.Join(",", names)
            };

            var result = cut.Parse(args);

            Assert.AreEqual(State.Succeeded, result.State);
            Assert.IsInstanceOfType(result.Option, typeof(CollectionOption));

            var option = (CollectionOption)result.Option;

            Assert.AreEqual(names.Length, option.Names.Length);
            for (var i = 0; i < names.Length; i++)
            {
                Assert.AreEqual(names[i], option.Names[i], $"names differ at index {i}");
            }
        }
        [TestMethod]

        public void DefaultArray()
        {
            var cut = Parser.Create<CollectionOption>();

            var args = new string[0];

            var result = cut.Parse(args);

            Assert.AreEqual(State.Succeeded, result.State);
            Assert.IsInstanceOfType(result.Option, typeof(CollectionOption));

            var option = (CollectionOption)result.Option;

            Assert.AreEqual(1, option.Names.Length);
            Assert.AreEqual("John", option.Names[0]);            
        }
    }
}
