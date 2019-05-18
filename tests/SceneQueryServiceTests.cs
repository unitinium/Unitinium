using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Unitinium.Tests
{
    public class SceneQueryServiceTests
    {
        [Test]
        public void Execute_ShouldExecuteVisitor()
        {
            // Arrange
            var tokens = new[] {"name"};
            var ast = new[] {new QueryNameAst() {Value = "name"}};
            var lexer = new LexerMock(tokens, "name");
            var parser = new ParserMock(tokens, ast);
            var defaultData = new[] {"data"};
            var visitor = new AstVisitorMock(ast[0], defaultData, "data");
            var visitors = new Dictionary<Type, IQueryAstVisitor>()
            {
                [typeof(QueryNameAst)] = visitor
            };
            var wrapper = new RuntimeWrapperServiceMock("data", true, "wrap(data)");
            var queryService = new SceneQueryService(lexer, parser, visitors, wrapper, defaultData);

            // Act
            var result = queryService.Execute("name");

            // Assert
            Assert.AreEqual("wrap(data)", result);
        }
    }

    public class RuntimeWrapperServiceMock : IRuntimeObjectWrapperService
    {
        public object Value;
        public bool IsDeep;
        public object Return;

        public RuntimeWrapperServiceMock(object value, bool isDeep, object @return)
        {
            Value = value;
            IsDeep = isDeep;
            Return = @return;
        }

        public object Wrap(object value, bool isDeep = true, bool isChield = true)
        {
            if(value != Value || isDeep != IsDeep)
            {
                throw new Exception();
            }

            return Return;
        }
    }
}