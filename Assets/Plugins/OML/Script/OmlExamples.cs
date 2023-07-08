using OmlUtility;
using UnityEngine;


namespace OmlExampleNS
{
    public class Example1_Tag_GameObject : MonoBehaviour
    {
        public Example1_Tag_GameObject()
        {
            Oml.TagObject(this, "Example1");

            Debug.Log("This message will be auto tagged with [Example1].", this);

            //Debug.Log("This message will NOT be auto tagged.");
        }
    }

    /// <summary>
    /// We are using extension method to log.
    /// Don't need to pass 'this' to the log functon.
    /// </summary>
    public class Example2_Tag_GameObject : ITagObject
    {
        public Example2_Tag_GameObject()
        {
            Oml.TagObject(this, "Example2");

            this.Log("This message will be auto tagged with [Example2].");

            //Debug.Log("This message will NOT be auto tagged.");
        }
    }


    /// <summary>
    /// Example of using LogRouter class to tag log with an object of choice.
    /// In this case, the provide 'this' to LogRouter.
    /// 
    /// Adventage: you don't need to pass the instance (this) to the log function.
    /// </summary>
    public class Example3_Tag_AnyObject
    {
        // with this, Debug is no longer the UnityEngine.Debug
        LogRouter Debug = null;

        public Example3_Tag_AnyObject()
        {
            Oml.TagObject(this, "AnyObjectTag");

            Debug = new LogRouter(this);

            Debug.Log("This message will be auto tagged with [AnyObjectTag].");

            //Debug.Log("This message will NOT be auto tagged.");
        }
    }

    /// <summary>
    /// This example pass another object to LogRouter
    /// Logs will be borrowing object-tags from that class of Example3_Tag_AnyObject;
    /// 
    /// 
    /// </summary>
    public class Example4_Tag_Routing
    {
        Example3_Tag_AnyObject sampleObject = new Example3_Tag_AnyObject();

        // with this, Debug is no longer the UnityEngine.Debug
        LogRouter Debug = null;

        public Example4_Tag_Routing()
        {
            Debug = new LogRouter(sampleObject);

            Debug.Log("This message will auto be tagged [Example].");

            //Debug.Log("This message will NOT be auto tagged.");
        }
    }

}
