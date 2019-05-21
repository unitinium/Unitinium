using System.Collections;
using System.Linq;

namespace Unitinium
{
    public class QueryIndexAstVisitor : IQueryAstVisitor
    {
        public object Visit(QueryAstBase node, object currentData)
        {
            if (!(currentData is IEnumerable dataEnumerable))
            {
                return null;
            }
            
            var casted = dataEnumerable.OfType<object>();
            var index = (int)node.Value;
            if (index < 0)
            {
                var reversed = casted.Reverse();
                return reversed.ElementAt(index);
            }
            return casted.ElementAt(index);

        }
    }
}