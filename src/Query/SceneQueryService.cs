using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Unitinium
{
    public class SceneQueryService : IQueryService
    {
        public IQueryLexer Lexer { get; set; }
        public IQueryParser Parser { get; set; }
        public IDictionary<Type, IQueryAstVisitor> Visitors { get; set; }
        public IRuntimeObjectWrapperService Wrapper { get; set; }
        public object DefaultGlobalObject { get; set; }

        public SceneQueryService(IQueryLexer lexer, IQueryParser parser, 
            IDictionary<Type, IQueryAstVisitor> visitors, IRuntimeObjectWrapperService wrapper,
            object defaultGlobalObject)
        {
            Lexer = lexer;
            Parser = parser;
            Visitors = visitors;
            Wrapper = wrapper;
            DefaultGlobalObject = defaultGlobalObject;
        }

        public SceneQueryService() : 
            this(new QueryLexer(), 
                new QueryParser(), 
                new Dictionary<Type, IQueryAstVisitor>(), 
                new RuntimeObjectWrapperService(),
                SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Visitors[typeof(QueryGlobalAst)] = new QueryGlobalAstVisitor();
            Visitors[typeof(QueryComponentAst)] = new QueryComponentAstVisitor();
            Visitors[typeof(QueryIndexAst)] = new QueryIndexAstVisitor();
            Visitors[typeof(QueryNameAst)] = new QueryNameAstVisitor();
            Visitors[typeof(QueryFirstExpandAst)] = new QueryFirstExpandAstVisitor();
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
            
            var wrapped = Wrapper.Wrap(result);
            return wrapped;
        }
    }
}