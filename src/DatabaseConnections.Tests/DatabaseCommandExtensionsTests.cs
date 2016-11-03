using NUnit.Framework;

namespace DatabaseConnections.Tests
{
    [TestFixture]
    [Parallelizable]
    public class DatabaseCommandExtensionsTests
    {
        [Test]
        public void Compile_Should_EscapeSingleQuotes()
        {
            var command = new DatabaseCommand
            {
                CommandText = "SELECT * FROM [test] WHERE [name]=@name",
                Parameters = new[]
                {
                    new DbParam("@name", "Trevor O'Tool")
                }
            };

            var result = command.Compile();

            Assert.AreEqual("SELECT * FROM [test] WHERE [name]='Trevor O\\'Tool'", result);
        }

        [Test]
        public void Compile_ShouldReplace_ParametersWithValues()
        {
            var command = new DatabaseCommand
            {
                CommandText = "SELECT * FROM [test] WHERE [id]=@id OR [name]=@name",
                Parameters = new[]
                {
                    new DbParam("@id", 1),
                    new DbParam("@name", "My Name")
                }
            };

            var result = command.Compile();

            Assert.AreEqual("SELECT * FROM [test] WHERE [id]=1 OR [name]='My Name'", result);
        }
    }
}