using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace Unitinium
{
    public class LuaRuntimeExecution : ILuaRuntimeExecution
    {
        public int Id { get; private set; }

        public RuntimeExecutionState State { get; private set; }

        public object Result { get; private set; }

        private DynValue _coroutine;

        private Func<bool> resumeRequester;

        public LuaRuntimeExecution(int id, Script script, DynValue function)
        {
            Id = id;
            _coroutine = script.CreateCoroutine(function);
        }

        public void Update()
        {
            if(_coroutine.Coroutine.State == CoroutineState.Dead)
            {
                return;
            }

            if(resumeRequester != null && !resumeRequester())
            {
                return;
            }

            var value = _coroutine.Coroutine.Resume();

            CheckRequester(value);
            
            TryFinalize(value);
        }

        private void CheckRequester(DynValue value)
        {
            var clrObject = value.ToObject();

            if (clrObject is LuaCoroutineYield @yield)
            {
                resumeRequester = @yield.GetRequester();
            }
        }

        private void TryFinalize(DynValue value)
        {
            object correctValue = null;
            if(value.Table != null)
            {
                correctValue = value.ToObject<IDictionary<object, object>>();
            }
            else
            {
                correctValue = value.ToObject();
            }

            if(_coroutine.Coroutine.State == CoroutineState.Dead)
            {
                Result = correctValue;
                State = RuntimeExecutionState.Success;
            }
        }
    }
}