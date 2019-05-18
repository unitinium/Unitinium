using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unitinium
{
    public class QueryFirstExpandAstVisitor : IQueryAstVisitor
    {
        public object Visit(QueryAstBase node, object currentData)
        {
            if (currentData is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().FirstOrDefault();
            }
            
            if (currentData is GameObject gameObject)
            {
                return gameObject.transform
                    .Cast<Transform>()
                    .Select(t => t.gameObject)
                    .ToArray();
            }

            if (currentData is Component component)
            {
                return currentData.GetType()
                    .GetTypeInfo()
                    .GetMembers(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty)
                    .ToArray();
            }

            if (currentData is Scene scene)
            {
                return scene.GetRootGameObjects();
            }

            return null;
        }
    }
}