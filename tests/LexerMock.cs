using System;

namespace Unitinium.Tests
{
    public class LexerMock : IQueryLexer
    {
        public string Query;
        public object[] ReturnValue;

        public LexerMock(object[] returnValue, string query)
        {
            ReturnValue = returnValue;
            Query = query;
        }

        public object[] Tokenize(string query)
        {
            if (!Query.Equals(query))
            {
                throw new Exception();
            }
            
            return ReturnValue;
        }
    }
}