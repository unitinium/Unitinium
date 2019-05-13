using System.Linq;
using NUnit.Framework;

namespace Unitinium.Tests
{
    public class QueryLexerTests
    {
        [Test]
        public void Parse_ShouldNormalParseText()
        {
            // Arrange
            var lexer = new QueryLexer();
            var input = "GameManager#Camera.fieldOfView";
            var output = new object[]
            {
                "GameManager", 
                new QuerySpecialToken("#"),
                "Camera",
                new QuerySpecialToken("."),
                "fieldOfView"
            };

            // Act
            var result = lexer.Tokenize(input);

            // Assert
            Assert.True(Enumerable.SequenceEqual(output, result));
        }
        
        [Test]
        public void Parse_ShouldNormalParseText1()
        {
            // Arrange
            var lexer = new QueryLexer();
            var input = @"GO[1]";
            var output = new object[]
            {
                "GO",
                new QuerySpecialToken("["),
                1,
                new QuerySpecialToken("]")
            };

            // Act
            var result = lexer.Tokenize(input);

            // Assert
            Assert.True(Enumerable.SequenceEqual(output, result));
        }
        
        [Test]
        public void Parse_ShouldNormalParseText2()
        {
            // Arrange
            var lexer = new QueryLexer();
            var input = @"Esc\a\\ping\#\.Test";
            var output = new object[] { @"Esca\ping#.Test" };

            // Act
            var result = lexer.Tokenize(input);

            // Assert
            Assert.True(Enumerable.SequenceEqual(output, result));
        }
    }
}
