#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins.Internal
{
    public class AndroidPluginUtility
    {
        private static Dictionary<string, AndroidJavaObject> sSingletonInstances = new Dictionary<string, AndroidJavaObject>();
        private static AndroidJavaObject context = null;

        public static AndroidJavaObject GetSingletonInstance(string _className, string _methodName = "getInstance") //Assuming the class follows standard naming- "INSTANCE" for singleton objects
        {
            AndroidJavaObject _instance;

            sSingletonInstances.TryGetValue(_className, out _instance);

            if (_instance == null)
            {
                //Create instance
                AndroidJavaClass _class = new AndroidJavaClass(_className);

                if (_class != null) //If it doesn't exist, throw an error
                {
                    _instance = _class.CallStatic<AndroidJavaObject>(_methodName);

                    //Add the new instance value for this class name key
                    sSingletonInstances.Add(_className, _instance);
                }
                else
                {
                    Debug.LogError("[NativePlugins] " + "Class=" + _className + " not found!");
                    return null;
                }

            }

            return _instance;
        }

        public static AndroidJavaClass CreateJavaClass(string className)
        {
            AndroidJavaClass javaClass;

            //Create instance
            javaClass = new AndroidJavaClass(className);

            if (javaClass == null) //If it doesn't exist, throw an error
            {
                Debug.LogError("[NativePlugins] " + "Class=" + className + " not found!");
            }

            return javaClass;
        }

        public static AndroidJavaObject CreateJavaInstance(string className, bool passContext = true)
        {
            AndroidJavaObject instance;

            //Create instance
            if (passContext)
            {
                instance = new AndroidJavaObject(className, GetContext());
            }
            else
            {
                instance = new AndroidJavaObject(className);
            }

            if (instance == null) //If it doesn't exist, throw an error
            {
                Debug.LogError("[NativePlugins] " + "Unable to create instance for class : "+ className);
            }

            return instance;
        }

        public static AndroidJavaObject GetContext()
        {
            if (context == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return context;
        }
    }
}
#endif