namespace Unitinium
{
    public interface IQueryAstVisitor
    {
        object Visit(QueryAstBase node, object currentData);
    }
}