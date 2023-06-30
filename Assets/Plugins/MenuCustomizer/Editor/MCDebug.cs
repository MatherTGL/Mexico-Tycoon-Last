using UnityEngine;

namespace MenuCustomizer
{
    /// <summary>wrapped method class of UnityEngine.Debug.*</summary>
    public static class MCDebug
    {
        /// <summary>whether output log or not</summary>
        static bool logOutput = true;

        /// <summary>property of 'logOutput'</summary>
        public static bool LogOutputEnable
        {
            get { return logOutput; }
            set { logOutput = value; }
        }

        /// <summary>wrapped method of Debug.Log()</summary>
        public static void Log(object message)
        {
            if(!logOutput) { return; }
            Debug.Log(message);
        }

        /// <summary>wrapped method of Debug.LogError()</summary>
        public static void LogError(object message)
        {
            if(!logOutput) { return; }
            Debug.LogError(message);
        }

        /// <summary>wrapped method of Debug.LogWarning()</summary>
        public static void LogWarning(object message)
        {
            if(!logOutput) { return; }
            Debug.LogWarning(message);
        }
    }
}
