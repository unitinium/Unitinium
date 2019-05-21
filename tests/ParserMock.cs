using System;

namespace Unitinium.Tests
{
    public class ParserMock : IQueryParser
    {
        public object[] Tokens;
        public QueryAstBase[] ReturnValue;

        public ParserMock(object[] tokens, QueryAstBase[] returnValue)
        {
            Tokens = tokens;
            ReturnValue = returnValue;
        }

        public QueryAstBase[] Parse(object[] tokens)
        {
            if (Tokens != tokens)
            {
                throw new Exception();
            }

            return ReturnValue;
        }
    }
}