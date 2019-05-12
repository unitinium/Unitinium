using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unitinium
{
    public class SceneDumpService
    {
        public SceneNode DumpScenes()
        {
            var result = new SceneNode()
            {
                name = "world",
                childs = Enumerable.Range(0, SceneManager.sceneCount)
                    .Select(i => SceneManager.GetSceneAt(i))
                    .Select(s => new SceneNode()
                    {
                        name = s.name,
                        childs = s.GetRootGameObjects()
                            .Select(o => TransformToSceneObject(o.transform))
                            .ToList()
                    }).ToList()
            };

            return result;
        }

        private SceneNode TransformToSceneObject(Transform obj)
        {
            var components = obj.gameObject.GetComponents(typeof(Component));
            var componentMap = components.Select(SerializeComponent)
                .ToArray();

            var result = new SceneNode()
            {
                name = obj.name,
                components = componentMap
            };

            foreach(Transform currentTransform in obj)
            {
                result.childs.Add(TransformToSceneObject(currentTransform));
            }

            return result;
        }

        private Dictionary<string, object> SerializeComponent(Component component)
        {
            var result = new Dictionary<string, object>();
            result["__type"] = component.GetType().FullName;

            var cTransform = component as Transform;
            if(cTransform != null)
            {
                result["position"] = ToDictionary(cTransform.position);
                result["rotation"] = ToDictionary(cTransform.rotation);
                result["scale"] = ToDictionary(cTransform.localScale);
                return result;
            }

            if(!(component is MonoBehaviour))
            {
                return result;
            }

            result["__type"] = component.GetType().FullName;
            result = ToDictionary(component);

            return result;
        }

        private Dictionary<string, object> ToDictionary(object source)
        {
            var json = JsonUtility.ToJson(source);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
    }
}