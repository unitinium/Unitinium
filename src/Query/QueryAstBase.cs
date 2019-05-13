namespace Unitinium
{
    public abstract class QueryAstBase
    {
        public object Value { get; set; }

        public override string ToString()
        {
            return $"{Value}({GetType().Name})";
        }
        
        public override bool Equals(object obj)
        {
            if (obj?.GetType() == GetType())
            {
                var casted = (QueryAstBase) obj;
                if (casted.Value == null && Value == null)
                {
                    return true;
                }
                
                var idEquals = casted.Value?.Equals(Value);
                return idEquals.HasValue ? idEquals.Value : false;
            }
            return base.Equals(obj);
        }
    }
    
    public class QueryNameAst : QueryAstBase
    {
    }
    
    public class QueryGlobalAst : QueryAstBase
    {
    }
    
    public class QueryComponentAst : QueryAstBase
    {
    }
    
    public class QueryFirstExpandAst : QueryAstBase
    {
    }
    
    public class QueryIndexAst : QueryAstBase
    {
    }
}