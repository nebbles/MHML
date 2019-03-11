#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    public static class JNIUtility
    {
        public delegate T NativeJavaObjectConverter<T>(AndroidJavaObject nativeObject);

        public static List<T> GetList<T>(this AndroidJavaObject nativeObject, NativeJavaObjectConverter<T> converter)
        {
            if (nativeObject == null)
                return null;

            int size = nativeObject.Call<int>("size");

            List<T> list = new List<T>();
            for (int eachIndex = 0; eachIndex < size; eachIndex++)
            {
                AndroidJNI.PushLocalFrame(128);
                using (AndroidJavaObject eachNativeObject = nativeObject.Call<AndroidJavaObject>("get", eachIndex))
                {
                    T newObject = converter(eachNativeObject);
                    list.Add(newObject);
                }
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }

            return list;
        }

        public static string GetString(this AndroidJavaObject javaObject)
        {
            if (javaObject == null)
                return null;

            return javaObject.Call<string>("toString");
        }
    }
}
#endif
