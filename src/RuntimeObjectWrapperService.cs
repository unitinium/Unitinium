using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System.ComponentModel;
using System;

namespace Unitinium
{
    public class RuntimeObjectWrapperService : IRuntimeObjectWrapperService
    {
        public object Wrap(object value, bool isDeep = true, bool needChild = true)
        {
            return new WrapContext().Wrap(value, isDeep, needChild);
        }
    }

    public class WrapContext
    {
        public Dictionary<object, Dictionary<string, object>> _visitedObjects = new Dictionary<object, Dictionary<string, object>>();

        public object Wrap(object value, bool isDeep = true, bool needChild = true)
        {
            if(value is Transform transform)
            {
                return TransformToSceneObject(transform, isDeep, needChild);
            }

            if(value is GameObject go)
            {
                return TransformToSceneObject(go.transform, isDeep, needChild);
            }

            if(value is UnityEngine.Component comp)
            {
                return ToDictionary(comp);
            }

            if(value is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().Select(v => Wrap(v)).ToList();
            }

            if(value is TypeMemberInstance instance)
            {
                return new Dictionary<string, object>()
                {
                    ["__type"] = nameof(TypeMemberInstance),
                    ["name"] = instance.Name,
                    ["value"] = Wrap(instance.GetType(), false),
                    ["source"] = Wrap(instance.Instance, false)
                };
            }

            if (value == null)
            {
                return null;
            }

            if(IsPrimitiveType(value.GetType()))
            {
                return value;
            }

            try
            {
                return ToDictionary(value);
            }
            catch
            {
            }

            return value;
        }

        private bool IsPrimitiveType(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return type == typeof(string) 
                || typeInfo.IsPrimitive
                || typeInfo.IsEnum
                || (typeInfo.IsArray && IsPrimitiveType(typeInfo.GetElementType()));
        }

        SceneNode TransformToSceneObject(Transform obj, bool isDeep = true, bool needChidls = true)
        {
            var components = obj.gameObject.GetComponents(typeof(UnityEngine.Component));
            var componentMap = components.Select(SerializeComponent)
                .ToArray();

            var result = new SceneNode()
            {
                name = obj.name,
                components = componentMap,
                instanceId = obj.gameObject.GetInstanceID()
            };

            if(needChidls || isDeep)
            {
                foreach(Transform currentTransform in obj)
                {
                    result.childs.Add(TransformToSceneObject(currentTransform, isDeep, isDeep ? needChidls : false));
                }
            }
            else
            {
                result.childs = null;
            }

            return result;
        }

        Dictionary<string, object> SerializeComponent(UnityEngine.Component component)
        {
            var result = new Dictionary<string, object>();
            result["$type"] = component.GetType().FullName;
            result["$instanceId"] = component.GetInstanceID();

            var data = new Dictionary<string, object>();
            result["data"] = data;

            var cTransform = component as Transform;
            if(cTransform != null)
            {
                data["position"] = ToDictionary(cTransform.position);
                data["rotation"] = ToDictionary(cTransform.rotation);
                data["scale"] = ToDictionary(cTransform.localScale);
                return result;
            }

            result["data"] = ToDictionary(component);

            return result;
        }

        Dictionary<string, object> ToDictionary(object source)
        {
            if (_visitedObjects.ContainsKey(source))
            {
                return new Dictionary<string, object>()
                {
                    ["$ref"] = source.GetHashCode()
                };
            }


            var data = new Dictionary<string, object>();
            data["$id"] = source.GetHashCode();
            data["$type"] = source.GetType().FullName;

            _visitedObjects[source] = data;

            try
            {
                var json = JsonUtility.ToJson(source);
                var dataUnity = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if(dataUnity != null)
                {
                    Merge(dataUnity, data);
                }
            }
            catch {}

            if(data.Count > 2)
            {
                return data;
            }

            var props = source.GetType().GetTypeInfo()
                .GetMembers(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => m is PropertyInfo || m is FieldInfo)
                .Where(m => !m.GetCustomAttributes<EditorBrowsableAttribute>().Any(k => k.State == EditorBrowsableState.Never))
                .Where(m => !m.Name.Equals("gameObject"))
                .Select(m => new TypeMemberInstance(m, source));
            
            var dataRaw = props.ToDictionary(p => p.Name, p => {
                if(source == p.GetValue())
                {
                    return data;
                }
                return Wrap(p.GetValue(), false, false);
            });

            Merge(dataRaw, data);

            return data;
        }

        void Merge(Dictionary<string, object> source, Dictionary<string, object> destination)
        {
            foreach (var kv in source)
            {
                destination[kv.Key] = kv.Value;
            }
        }
    }
}