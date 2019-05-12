using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unitinium.Tests
{
    public class QueryLexerTests
    {
        [Test]
        [TestCase("GameManager#Camera.fieldOfView", new object[] {"GameManager", "#", "Camera", ".", "fieldOfView"})]
        [TestCase(@"Esc\a\\ping\#\.Test", new [] {@"Esca\ping#.Test"})]
        [TestCase(@"GO[1]", new [] {@"GO", "[", "1", "]"})]
        public void Parse_ShouldNormalParseText(string input, string[] output)
        {
            // Arrange
            var lexer = new QueryLexer();

            // Act
            var result = lexer.Parse(input);

            // Assert
            Log(string.Join(", ", result));
            Assert.True(Enumerable.SequenceEqual(output, result));
        }

        void Log(string message)
        {
            LogAssert.Expect(LogType.Log, message);
            Debug.Log(message);
        }
    }
}
