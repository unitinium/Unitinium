using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityJsonRpc;
using Random = UnityEngine.Random;

namespace Unitinium
{
    public class UnitiniumService : MonoBehaviour
    {
        public int ListenPort = 8080;

        private JsonRpcServer server;
        private ConcurrentQueue<JsonRpcRequest> requests;
        private Dictionary<string, Action<JsonRpcRequest>> methods;
        
        public void Awake()
        {
            methods = new Dictionary<string, Action<JsonRpcRequest>>()
            {
                ["scenesDump"] = sceneDump
            };
            server = new JsonRpcServer();
            requests = server.Start("http://localhost:" + ListenPort + "/");
        }

        private void sceneDump(JsonRpcRequest obj)
        {
            obj.Respond(DumpScenes());
        }

        private Dictionary<string, object> DumpScenes()
        {
            var result = new Dictionary<string, object>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                result[scene.name] = "";
            }

            return result;
        }

        void Update()
        {
            if (requests.IsEmpty)
            {
                return;
            }

            if (requests.TryDequeue(out var request))
            {
                var name = request.RequestData.method;
                if (methods.ContainsKey(name))
                {
                    methods[name](request);
                }
                else
                {
                    request.Error("method not found");
                }
            }
        }
        
        public void OnDestroy()
        { 
            server.Stop();
        }
    }

    internal class SceneObject
    {
        public string name { get; set; }
        public SceneObject[] childs { get; set; }
    }
}