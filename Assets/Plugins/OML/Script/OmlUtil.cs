using UnityEngine;

#if !LIB_BUILD



public class Oml
{
    public static void HelloWorld() => Debug.Log("Hello World");
    

    public static void TagObject(Object obj, string tags)
    {
        tags = tags.Replace(" ", string.Empty);
        Debug.Log( "/tag " + tags, obj);
    }

    public static void TagObject(object obj, string tags)
    {
        var hash = obj.GetHashCode();

        tags = tags.Replace(" ", string.Empty);

        Debug.Log($"/tag {tags} {hash}");
    }

    public static void BeginSession(string name, bool main = true)
    {
        Debug.Log( 
            (main ? "/main-session " : "/session ")
            + name);
    }

    /// <summary>
    /// NOTE: 
    /// You don't need to end the last one if you only need to start new MAIN session.
    /// Starting a new Main Session automatically end the previous session.
    /// </summary>
    public static void EndSession(string name = "")
    {
        Debug.Log("/session-end " + name);
    }

}


namespace OmlUtility
{
    public interface ITagObject{}



    public static class OmlExtensions
    {
        public static void Log(this ITagObject obj, string msg) 
        {
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.Log(msg); 
        }

        public static void LogFormat(this ITagObject obj, string fmt, params object[] args) 
        {
            var msg = string.Format(fmt, args);
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.Log(msg);
        }

        public static void LogWarning(this ITagObject obj, string msg)
        {
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.LogWarning(msg);
        }

        public static void LogWarningFormat(this ITagObject obj, string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.LogWarning(msg);
        }

        public static void LogError(this ITagObject obj, string msg)
        {
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.LogError(msg);
        }

        public static void LogErrorFormat(this ITagObject obj, string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.LogError(msg);
        }

        public static void LogException(this ITagObject obj, System.Exception ex)
        {
            var msg = ex.Message;
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.LogError(msg);
        }

        public static void LogAssertion(this ITagObject obj, object msg) 
        {
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.LogAssertion(msg); 
        }

        public static void LogAssertionFormat(this ITagObject obj, string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            msg = $"®{obj.GetHashCode()}®" + msg;
            Debug.LogAssertion(msg);
        }
    }



    /// <summary>
    /// Use this class inside class where it does not extend GameObject,MonoHehaviour
    /// 
    /// Name the variable Debug to minimize code change.
    /// 
    /// Check out OmlExamples.cs for sample code
    /// </summary>
    public class LogRouter
    {
        object target = null;
        
        public LogRouter(object target)
        {
            this.target = target;
        }

        #region [ ========== Forward ========== ]

        public void DrawLine(Vector3 start, Vector3 end)
        {
            Debug.DrawLine(start, end);
        }

        public void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            Debug.DrawLine(start, end, color);
        }

        public void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            Debug.DrawLine(start, end, color, duration);
        }

        public void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }

        public void DrawRay(Vector3 start, Vector3 dir)
        {
            Debug.DrawRay(start, dir);
        }

        public void DrawRay(Vector3 start, Vector3 dir, Color color)
        {
            Debug.DrawRay(start, dir, color);
        }

        public void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
        {
            Debug.DrawRay(start, dir, color, duration);
        }

        public void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
        {
            Debug.DrawRay(start, dir, color, duration, depthTest);
        }
        public void ClearDeveloperConsole() => Debug.ClearDeveloperConsole();
        
        public bool developerConsoleVisible
        {
             get => Debug.developerConsoleVisible;
             set => Debug.developerConsoleVisible = value;
        }

        public void Assert(bool condition) => Debug.Assert(condition);
        

        public void AssertFormat(bool condition, string fmt, params object[] args)
        {
            Debug.AssertFormat(condition, fmt, args);
        }

        public void Break() => Debug.Break();

        #endregion [ ========== Forward (End) ========== ]


        #region [ ========== Log Functions ========== ]

        public void Log(string msg)
        {
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.Log(msg);
        }

        public void LogFormat(string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.Log(msg);
        }

        public void LogWarning(string msg)
        {
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.LogWarning(msg);
        }

        public void LogWarningFormat(string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.LogWarning(msg);
        }

        public void LogError(string msg)
        {
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.LogError(msg);
        }

        public void LogErrorFormat(string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.LogError(msg);
        }

        public void LogException(System.Exception ex)
        {
            var msg = ex.Message;
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.LogError(msg);
        }

        public void LogAssertion(object msg)
        {
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.LogAssertion(msg);
        }

        public void LogAssertionFormat(string fmt, params object[] args)
        {
            var msg = string.Format(fmt, args);
            msg = $"®{target.GetHashCode()}®" + msg;
            Debug.LogAssertion(msg);
        }

        #endregion [ ========== Log Functions (End) ========== ]
        
    }

    ///// <summary>
    ///// Option 1 
    ///// In case you are using TaggedLogger but want to disable auto-tag in release build,
    ///// you can use #if #else #endif to switch the TaggedLogger with ForwardLogger 
    ///// so that it will just do log message forwarding.
    ///// 
    ///// 
    ///// Don't forget to add 'OmlUtility' to stack frame filter list 
    ///// in OML setting panel "stack trace" section.
    ///// 
    ///// 
    ///// 
    ///// Option 2: 
    ///// 
    ///// Add this code below to the top of the code files to do the redirection. 
    ///// 
    /////     using logger = UnityEngine.Debug;
    ///// 
    ///// 'logger' is the original instance name of TaggedLogger. The original can be commented out.
    ///// 
    ///// In this way, you can save a few bytes per object of holding the logger class 
    ///// without using the ForwardLogger. 
    ///// Use this good when you are writing server with thausands and thausands of objects.
    ///// </summary>
    //public class ForwardLogger : Object
    //{
    //    public ForwardLogger(string tagName = "")
    //    {
    //        // tagName is not use here. just for having the same arg as TaggedLogger
    //    }

    //    public void Log(string msg) { Debug.Log(msg); }

    //    public void LogFormat(string fmt, params object[] args) { Debug.LogFormat( fmt, args); }

    //    public void LogWarning(string msg) { Debug.LogWarning(msg); }

    //    public void LogWarningFormat(string fmt, params object[] args) { Debug.LogWarningFormat( fmt, args); }

    //    public void LogError(string msg) { Debug.LogError(msg); }

    //    public void LogErrorFormat(string fmt, params object[] args) { Debug.LogErrorFormat( fmt, args); }

    //    public void LogException(System.Exception ex) { Debug.LogException(ex); }

    //    public void LogAssertion(object msg) { Debug.LogAssertion(msg); }
    //    public void LogAssertionFormat(string fmt, params object[] args) { Debug.LogAssertionFormat(fmt, args); }
    //}

#endif
}