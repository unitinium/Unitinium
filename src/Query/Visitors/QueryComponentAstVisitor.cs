using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Unitinium
{
    public class QueryComponentAstVisitor : IQueryAstVisitor
    {
        public object Visit(QueryAstBase node, object currentData)
        {
            var type = Type.GetType(node.Value.ToString(), true, true);
            
            if (currentData is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().Where(comp => type == comp.GetType());
            }

            if (currentData is GameObject go)
            {
                return go.GetComponents(type);
            }

            return null;
        }
    }
}