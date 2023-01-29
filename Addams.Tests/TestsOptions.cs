using NUnit.Framework;

namespace Addams.Tests
{
    public class TestsOptions
    {
        [Test]
        public void TestArgumentsWithNoOptionsValid()
        {
            string[] args = new string[] { };
            AddamsOptions opt = AddamsOptions.DefineOptions(args);

            Assert.False(opt.Debug);
        }

        [Test]
        public void TestArgumentsWithDebugOptions()
        {
            string[] args = new string[] { "--debug" };
            AddamsOptions opt = AddamsOptions.DefineOptions(args);

            Assert.True(opt.Debug);
        }
    }
}