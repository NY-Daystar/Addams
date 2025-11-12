using NUnit.Framework;

namespace Addams.Tests;

[TestFixture]
public class TestsOptions
{
    [Test]
    public void TestArgumentsWithNoOptionsValid()
    {
        // Arrange
        string[] args = new string[] { };

        // Act
        AddamsOptions opt = AddamsOptions.DefineOptions(args);

        // Assert
        Assert.False(opt.Debug);
    }

    [Test]
    public void TestArgumentsWithDebugOptions()
    {
        // Arrange
        string[] args = new [] { "--debug" };

        // Act
        AddamsOptions opt = AddamsOptions.DefineOptions(args);

        // Assert
        Assert.True(opt.Debug);
    }
}