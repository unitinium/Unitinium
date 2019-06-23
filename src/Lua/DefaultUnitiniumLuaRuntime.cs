using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Unitinium
{
    public class DefaultUnitiniumLuaRuntime : IUnitiniumLuaRuntime
    {
        public Script GlobalScript { get; set; }
        public readonly List<LuaRuntimeExecution> Executions;

        public DefaultUnitiniumLuaRuntime(IQueryService query)
        {
            Executions = new List<LuaRuntimeExecution>();
            GlobalScript = new Script();

            UserData.RegisterType<GameObject>();
            UserData.RegisterType<Debug>();
            UserData.RegisterType<Type>();
            UserData.RegisterType<IQueryService>();
            UserData.RegisterType<SceneNode>();
            UserData.RegisterType<LuaRuntimeExecution>();
            UserData.RegisterType<LuaCoroutineYield>();
            UserData.RegisterType<WaitLuaCoroutineYield>();


            GlobalScript.Globals["GameObject"] = UserData.CreateStatic<GameObject>();
            GlobalScript.Globals["Debug"] = UserData.CreateStatic<Debug>();
            GlobalScript.Globals["Type"] = UserData.CreateStatic<Type>();
            GlobalScript.Globals["query"] = query;
            GlobalScript.Globals["yield"] = GlobalScript.DoString(@"return |_| coroutine.yield(_)");
            GlobalScript.Globals["__wait_s"] = (Func<float, LuaCoroutineYield>)((s) => new WaitLuaCoroutineYield(s));
            GlobalScript.Globals["wait_s"] = GlobalScript.DoString(@"return |_| yield(__wait_s(_))");

            Script.GlobalOptions
                .CustomConverters
                .SetScriptToClrCustomConversion(DataType.Table, typeof(IDictionary<object, object>),
                    v => {
                        return v.Table.Pairs.ToDictionary(k => k.Key.ToObject(), vo => vo.Value.ToObject());
                    }
                );
        }

        public object Execute(string script, bool isCoroutine = false)
        {
            var result = GlobalScript.DoString(script);

            if(isCoroutine)
            {
                return CreateCoroutine(result);
            }

            if(result.Table != null)
            {
                return result.ToObject<IDictionary<object, object>>();
            }

            return result.ToObject();
        }

        public ILuaRuntimeExecution GetExecution(int id)
        {
            return Executions.FirstOrDefault(e => e.Id == id);
        }

        public void UpdateCoroutines()
        {
            for (int i = 0; i < Executions.Count; i++)
            {
                var item = Executions[i];
                item.Update();
            }
        }

        private LuaRuntimeExecution CreateCoroutine(DynValue result)
        {
            LuaRuntimeExecution execution = null;
            lock(Executions)
            {
                var id = 0;
                if(Executions.Any())
                {
                    id = Executions.Max(i => i.Id) + 1;
                }

                execution = new LuaRuntimeExecution(id, GlobalScript, result);
            }

            Schedule(execution);

            return execution;
        }

        private void Schedule(LuaRuntimeExecution execution)
        {
            Executions.Add(execution);
        }
    }
}