namespace Unitinium
{
    public interface IQueryParser
    {
        QueryAstBase[] Parse(object[] tokens);
    }
}