using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Unitinium
{
    public class QueryNameAstVisitor : IQueryAstVisitor
    {
        public object Visit(QueryAstBase node, object currentData)
        {
            if (currentData is GameObject gameObject)
            {
                return gameObject.transform
                    .Cast<Transform>()
                    .Select(t => t.gameObject)
                    .Where(g => g.name == node.Value.ToString())
                    .ToArray();
            }

            if (currentData is Component component)
            {
                return currentData.GetType()
                    .GetTypeInfo()
                    .GetMembers(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty)
                    .Where(s => s.Name == node.Value.ToString())
                    .ToArray();
            }

            return new object[0];
        }
    }
}