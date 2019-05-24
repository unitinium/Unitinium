using System;
using System.Collections.Concurrent;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityJsonRpc;

namespace Unitinium
{
    public class UnitiniumService : MonoBehaviour
    {
        public int ListenPort = 15263;

        private JsonRpcServer server;
        private ConcurrentQueue<JsonRpcRequest> requests;
        public JsonRpcProcessor Processor { get; set; }
        public SceneDumpService SceneDumpService { get; set; }
        public IUnitiniumLuaRuntime LuaRuntime { get; set; }
        public IQueryService QueryService { get; set; }

        public void Awake()
        {
            SceneDumpService = new SceneDumpService(new RuntimeObjectWrapperService());

            Processor = new JsonRpcProcessor();
            QueryService = new SceneQueryService();
            LuaRuntime = new DefaultUnitiniumLuaRuntime(QueryService);

            Processor.SetMethod("dump", SceneDumpService.DumpScenes);
            Processor.SetMethod("execute", (string script) => LuaRuntime.Execute(script));
            Processor.SetMethod("query", (string query) => QueryService.Execute(query));
            
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
                Processor.Handle(request);
            }
        }
        
        public void OnDestroy()
        { 
            server.Stop();
        }
    }
}