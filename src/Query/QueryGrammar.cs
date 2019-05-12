using System;
using System.Collections.Generic;

namespace Unitinium
{
    public class QueryGrammar
    {
        public QuerySyntaxTreeNode[] CreateSystaxTree(object[] tokens)
        {
            var tokenQueue = new Queue<object>(tokens);
            var nodeList = new List<QuerySyntaxTreeNode>();

            while (tokenQueue.Count > 0)
            {
                var token = tokenQueue.Dequeue();
                if (token is QuerySpecialToken specialToken)
                {
                    var node = ProcessOperator(specialToken, tokenQueue);
                    nodeList.Add(node);
                    continue;
                }

                string name = null;
                if (token is string idValue)
                {
                    name = idValue;
                }
                else if (token is int idIntValue)
                {
                    name = idIntValue.ToString();
                }

                if (name != null)
                {
                    nodeList.Add(new QueryNameSyntaxTreeNode
                    {
                        Value = name
                    });
                    continue;
                }
                
                throw new QueryGrammarException($"Not expected {token}");
            }

            return nodeList.ToArray();
        }

        private QuerySyntaxTreeNode ProcessOperator(QuerySpecialToken token, Queue<object> tokenQueue)
        {
            if (token.Value == "$")
            {
                return ProcessGlobaFilter(tokenQueue);
            }

            if (token.Value == "#")
            {
                return ProcessComponentFilter(tokenQueue);
            }
            
            if (token.Value == ".")
            {
                return new QueryFirstExpandSyntaxTreeNode();
            }
            
            if (token.Value == "[")
            {
                return ProcessIndex(tokenQueue);
            }
            
            throw new QueryGrammarException($"Not expected operator {token.Value}");
        }
        
        private QuerySyntaxTreeNode ProcessIndex(Queue<object> tokenQueue)
        {
            var intValue = Dequeue(tokenQueue, "number");
            var end = Dequeue(tokenQueue, "]");
            if (intValue is int && end is QuerySpecialToken endValue && endValue.Value == "]")
            {
                return new QueryIndexSyntaxTreeNode
                {
                    Value = intValue
                };
            }
            
            throw new QueryGrammarException($"Expected number in [] operator");
        }


        private QuerySyntaxTreeNode ProcessComponentFilter(Queue<object> tokenQueue)
        {
            var name = Dequeue(tokenQueue, "identifier");
            if (name is string value)
            {
                return new QueryComponentSyntaxTreeNode()
                {
                    Value = value
                };
            }
            
            throw new QueryGrammarException($"Expected identifier after # operator");
        }

        private QuerySyntaxTreeNode ProcessGlobaFilter(Queue<object> tokenQueue)
        {
            var name = tokenQueue.Dequeue();
            if (name is string | name is int)
            {
                return new QueryGlobalSyntaxTreeNode
                {
                    Value = name
                };
            }

            throw new QueryGrammarException($"Expected identifier after $ operator");
        }

        private object Dequeue(Queue<object> tokenQueue, string expected)
        {
            if (tokenQueue.Count <= 0)
            {
                throw new QueryGrammarException($"Ecpected {expected}, but fount end of query");
            }

            return tokenQueue.Dequeue();
        }
    }
}