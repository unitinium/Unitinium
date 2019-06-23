using System;
using UnityEngine;

namespace Unitinium
{
    public class WaitLuaCoroutineYield : LuaCoroutineYield
    {
        private readonly float _seconds;
        
        public WaitLuaCoroutineYield(float seconds)
        {
            _seconds = seconds;
        }

        public override Func<bool> GetRequester()
        {
            var endTime = Time.time + _seconds;
            return () => Time.time > endTime;
        }
    }
}