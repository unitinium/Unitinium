using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Unitinium
{
    public class QueryGlobalAstVisitor : IQueryAstVisitor
    {
        public object Visit(QueryAstBase node, object currentData)
        {
            if (node.Value.Equals("Scenes"))
            {
                return Enumerable.Range(0, SceneManager.sceneCount).Select(i => SceneManager.GetSceneAt(i));
            }

            if (node.Value.Equals("Components"))
            {
                return Object.FindObjectsOfType<Component>();
            }
            
            if (node.Value.Equals("RootGameObjects"))
            {
                return Enumerable
                    .Range(0, SceneManager.sceneCount)
                    .Select(i => SceneManager.GetSceneAt(i))
                    .SelectMany(s => s.GetRootGameObjects());
            }
            
            if (node.Value.Equals("GameObjects"))
            {
                return Object.FindObjectsOfType<Transform>()
                    .Select(o => o.gameObject);
            }

            return null;
        }
    }
}