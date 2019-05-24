using System;
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

            GlobalScript.Globals["GameObject"] = UserData.CreateStatic<GameObject>();
            GlobalScript.Globals["Debug"] = UserData.CreateStatic<Debug>();
            GlobalScript.Globals["Type"] = UserData.CreateStatic<Type>();
            GlobalScript.Globals["query"] = query;
        }

        public object Execute(string script)
        {
            var result = GlobalScript.DoString(script);
            return result.ToObject();
        }

        private void Log(string message)
        {
            Debug.Log(message);
        }
    }
}