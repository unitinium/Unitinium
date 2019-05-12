namespace Unitinium
{
    public abstract class QuerySyntaxTreeNode
    {
        public object Value { get; set; }

        public override string ToString()
        {
            return $"{Value}({GetType().Name})";
        }
    }
    
    public class QueryNameSyntaxTreeNode : QuerySyntaxTreeNode
    {
    }
    
    public class QueryGlobalSyntaxTreeNode : QuerySyntaxTreeNode
    {
    }
    
    public class QueryComponentSyntaxTreeNode : QuerySyntaxTreeNode
    {
    }
    
    public class QueryFirstExpandSyntaxTreeNode : QuerySyntaxTreeNode
    {
    }
    
    public class QueryIndexSyntaxTreeNode : QuerySyntaxTreeNode
    {
    }
}