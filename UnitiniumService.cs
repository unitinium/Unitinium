using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityJsonRpc;

namespace Unitinium
{
    public class UnitiniumService : MonoBehaviour
    {
        public int ListenPort = 8080;

        private JsonRpcServer server;
        private ConcurrentQueue<JsonRpcRequest> requests;
        private JsonRpcProcessor _processor;
        private SceneDumpService _sceneDumpService;

        public void Awake()
        {
            _sceneDumpService = new SceneDumpService();

            _processor = new JsonRpcProcessor();
            _processor.SetMethod("sceneDump", _sceneDumpService.DumpScenes);
            
            server = new JsonRpcServer();
            requests = server.Start("http://localhost:" + ListenPort + "/");
        }

        void Update()
        {
            if (requests.IsEmpty)
            {
                return;
            }

            if (requests.TryDequeue(out var request))
            {
                _processor.Handle(request);
            }
        }
        
        public void OnDestroy()
        { 
            server.Stop();
        }
    }
}