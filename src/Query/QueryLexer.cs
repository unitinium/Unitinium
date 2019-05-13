using System;
using System.Collections.Generic;

namespace Unitinium
{
    public class QueryLexer : IQueryLexer
    {
        public object[] Tokenize(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new NullReferenceException(query);
            }
            
            var result = new List<object>();
            var currentValue = "";
            var escapeMode = false;

            for (int i = 0; i < query.Length; i++)
            {
                var symbol = query[i];

                if (escapeMode)
                {
                    currentValue += symbol;
                    escapeMode = false;
                    continue;
                }

                if (symbol == '\\')
                {
                    escapeMode = true;
                }

                switch (symbol)
                {
                    case '\\':
                        escapeMode = true;
                        break;
                    
                    case '#':
                    case '.':
                    case '[':
                    case ']':
                    case '<':
                    case '$':
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            result.Add(ConvertValue(currentValue));
                        }
                        result.Add(new  QuerySpecialToken(symbol.ToString()));
                        currentValue = "";
                        break;
                    default:
                        currentValue += symbol.ToString();
                        break;
                }
            }
            
            if (!string.IsNullOrEmpty(currentValue))
            {
                result.Add(ConvertValue(currentValue));
            }
            
            return result.ToArray();
        }

        private object ConvertValue(string data)
        {
            if (int.TryParse(data, out var value))
            {
                return value;
            }

            return data;
        }
    }

    public class QuerySpecialToken
    {
        public string Value { get; set; }
        
        public QuerySpecialToken(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is QuerySpecialToken token && token.Value.Equals(Value))
            {
                return true;
            }
            
            return base.Equals(obj);
        }
    }
}