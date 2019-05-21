using System;

namespace Unitinium.Tests
{
    public class AstVisitorMock : IQueryAstVisitor
    {
        public QueryAstBase Node;
        public object CurrentData;
        public object Result;

        public AstVisitorMock(QueryAstBase node, object currentData, object result)
        {
            Node = node;
            CurrentData = currentData;
            Result = result;
        }

        public object Visit(QueryAstBase node, object currentData)
        {
            if (Node != node || CurrentData != currentData)
            {
                throw new Exception();
            }

            return Result;
        }
    }
}