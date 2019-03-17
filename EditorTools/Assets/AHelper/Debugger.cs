using UnityEngine;

namespace Tools
{
    public static class Debugger
    {
        public static void LogInfo(string message)
        {
            Debug.Log("[" + Timer.GetCurTimeMillis() + "]" + message);
        }
    }
}