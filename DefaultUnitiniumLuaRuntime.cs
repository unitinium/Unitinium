using System;
using System.Reflection;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Unitinium
{
    public class DefaultUnitiniumLuaRuntime : IUnitiniumLuaRuntime
    {
        public Script GlobalScript { get; set; }

        public DefaultUnitiniumLuaRuntime()
        {
            GlobalScript = new Script();

            UserData.RegisterType<GameObject>();
            UserData.RegisterType<Debug>();
            UserData.RegisterType<Type>();

            GlobalScript.Globals["GameObject"] = UserData.CreateStatic<GameObject>();
            GlobalScript.Globals["Debug"] = UserData.CreateStatic<Debug>();
            GlobalScript.Globals["Type"] = UserData.CreateStatic<Type>();
        }

        public void Execute(string script)
        {
            GlobalScript.DoString(script);
        }

        private void Log(string message)
        {
            Debug.Log(message);
        }
    }
}