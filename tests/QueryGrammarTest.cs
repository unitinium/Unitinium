using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unitinium.Tests
{
    public class QueryGrammarTests
    {
        [Test]
        public void Test1()
        {
            // Arrange
            var grammar = new QueryGrammar();
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
            
            // Act
            var result = grammar.CreateSystaxTree(tokens);
            
            // Assert
            Log(string.Join(", ", result.Select(t => t.ToString())));
        }
        
        void Log(string message)
        {
            LogAssert.Expect(LogType.Log, message);
            Debug.Log(message);
        }
    }
}