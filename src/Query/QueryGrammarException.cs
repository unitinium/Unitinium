using System;

namespace Unitinium
{
    [Serializable]
    public class QueryGrammarException : Exception
    {
        public QueryGrammarException(string message) : base(message)
        {
        }
    }
}