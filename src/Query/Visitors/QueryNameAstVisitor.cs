using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unitinium
{
    public class QueryNameAstVisitor : IQueryAstVisitor
    {
        public object Visit(QueryAstBase node, object currentData)
        {
            if(currentData is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().Where(i => {
                    if (i is GameObject iGo)
                    {
                        return iGo.name.Equals(node.Value);
                    }

                    if (i is Scene iScene)
                    {
                        return iScene.name.Equals(node.Value);
                    }

                    if (i is TypeMemberInstance iMI)
                    {
                        return iMI.Name.Equals(node.Value);
                    }

                    return false;
                });
            }

            if (currentData is Scene Scene)
            {
                return Scene.GetRootGameObjects()
                    .Where(g => g.name.Equals(node))
                    .ToArray();
            }

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
                    .Select(s => new TypeMemberInstance(s, currentData))
                    .ToArray();
            }

            return null;
        }
    }
}