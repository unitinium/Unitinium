using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Unitinium
{
    public class SceneQueryService
    {
        public IQueryLexer Lexer { get; set; }
        public IQueryParser Parser { get; set; }
        public IDictionary<Type, IQueryAstVisitor> Visitors { get; set; }
        public object DefaultGlobalObject { get; set; }

        public SceneQueryService(IQueryLexer lexer, IQueryParser parser, 
            IDictionary<Type, IQueryAstVisitor> visitors, object defaultGlobalObject)
        {
            Lexer = lexer;
            Parser = parser;
            Visitors = visitors;
            DefaultGlobalObject = defaultGlobalObject;
        }

        public SceneQueryService() : 
            this(new QueryLexer(), 
                new QueryParser(), 
                new Dictionary<Type, IQueryAstVisitor>(), 
                SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Visitors[typeof(QueryGlobalAst)] = new QueryGlobalAstVisitor();
            Visitors[typeof(QueryComponentAst)] = new QueryComponentAstVisitor();
            Visitors[typeof(QueryIndexAst)] = new QueryIndexAstVisitor();
            Visitors[typeof(QueryNameAst)] = new QueryNameAstVisitor();
        }
        
        public object Execute(string query)
        {
            var result = DefaultGlobalObject;
            var tokens = Lexer.Tokenize(query);
            var ast = Parser.Parse(tokens);
            
            foreach (var astNode in ast)
            {
                result = Visitors[astNode.GetType()].Visit(astNode, result);
            }
            
            return result;
        }
    }
}