using System;
using MoonSharp.Interpreter;

namespace Unitinium
{
    public abstract class LuaCoroutineYield
    {
        public abstract Func<bool> GetRequester();
    }
}