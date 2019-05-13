using System.Linq;
using NUnit.Framework;

namespace Unitinium.Tests
{
    public class QueryGrammarTests
    {
        [Test]
        public void CreateSystaxTree_EmptySequence()
        {
            // Arrange
            var grammar = new QueryParser();
            var tokens = new object[0];
            var expected = new QueryAstBase[0];
            
            // Act
            var result = grammar.Parse(tokens);
            
            // Assert
            Assert.True(Enumerable.SequenceEqual(result, expected));
        }
        
        [Test]
        public void CreateSystaxTree_CorrectSequence()
        {
            // Arrange
            var grammar = new QueryParser();
            var tokens = new object[]
            {
                "query",
                new QuerySpecialToken("."),
                new QuerySpecialToken("."),
                "query1",
            };
            
            var expected = new QueryAstBase[]
            {
                new QueryNameAst{Value = "query"},
                new QueryFirstExpandAst(),
                new QueryFirstExpandAst(),
                new QueryNameAst {Value = "query1"}
            };
            
            // Act
            var result = grammar.Parse(tokens);
            
            // Assert
            Assert.True(Enumerable.SequenceEqual(result, expected));
        }

        [Test]
        public void CreateSystaxTree_CorrectSequence1()
        {
            // Arrange
            var grammar = new QueryParser();
            var tokens = new object[]
            {
                new QuerySpecialToken("$"),
                "query1",
                new QuerySpecialToken("."),
                "query2",
                new QuerySpecialToken("#"),
                "query3",
                new QuerySpecialToken("["),
                1,
                new QuerySpecialToken("]"),
            };
            var expected = new QueryAstBase[]
            {
                new QueryGlobalAst{Value = "query1"},
                new QueryFirstExpandAst(),
                new QueryNameAst {Value = "query2"},
                new QueryComponentAst{Value = "query3"},
                new QueryIndexAst{Value = 1}
            };
            
            // Act
            var result = grammar.Parse(tokens);
            
            // Assert
            Assert.True(Enumerable.SequenceEqual(result, expected));
        }

        [Test]
        public void CreateSystaxTree_NotCorrectSequence()
        {
            // Arrange
            var grammar = new QueryParser();
            var tokens = new object[]
            {
                new QuerySpecialToken("#"),
            };

            // Act/Assert
            Assert.Catch<QueryGrammarException>(() => grammar.Parse(tokens));
        }
        
        [Test]
        public void CreateSystaxTree_NotCorrectSequence2()
        {
            // Arrange
            var grammar = new QueryParser();
            var tokens = new object[]
            {
                new QuerySpecialToken("["),
                1
            };

            // Act/Assert
            Assert.Catch<QueryGrammarException>(() => grammar.Parse(tokens));
        }
        
        [Test]
        public void CreateSystaxTree_NotCorrectSequence3()
        {
            // Arrange
            var grammar = new QueryParser();
            var tokens = new object[]
            {
                new QuerySpecialToken("["),
                "query"
            };

            // Act/Assert
            Assert.Catch<QueryGrammarException>(() => grammar.Parse(tokens));
        }
    }
}