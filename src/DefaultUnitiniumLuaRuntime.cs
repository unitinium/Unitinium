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

        public DefaultUnitiniumLuaRuntime(IQueryService query)
        {
            GlobalScript = new Script();

            UserData.RegisterType<GameObject>();
            UserData.RegisterType<Debug>();
            UserData.RegisterType<Type>();
            UserData.RegisterType<IQueryService>();
            UserData.RegisterType<SceneNode>();


            GlobalScript.Globals["GameObject"] = UserData.CreateStatic<GameObject>();
            GlobalScript.Globals["Debug"] = UserData.CreateStatic<Debug>();
            GlobalScript.Globals["Type"] = UserData.CreateStatic<Type>();
            GlobalScript.Globals["query"] = query;

            Script.GlobalOptions
                .CustomConverters
                .SetScriptToClrCustomConversion(DataType.Table, typeof(IDictionary<object, object>),
                    v => {
                        return v.Table.Pairs.ToDictionary(k => k.Key.ToObject(), vo => vo.Value.ToObject());
                    }
                );
        }

        public object Execute(string script)
        {
            var result = GlobalScript.DoString(script);

            if(result.Table != null)
            {
                return result.ToObject<IDictionary<object, object>>();
            }

            return result.ToObject();
        }

        private void Log(string message)
        {
            Debug.Log(message);
        }
    }
}