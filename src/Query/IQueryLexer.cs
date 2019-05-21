namespace Unitinium
{
    public interface IQueryLexer
    {
        object[] Tokenize(string query);
    }
}