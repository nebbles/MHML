#if UNITY_ANDROID
using UnityEngine;
using System;

namespace VoxelBusters.NativePlugins.Internal
{
    public class LoadURIProxyListener : AndroidJavaProxy
    {
        internal const string kPackage = "com.voxelbusters.nativeplugins.v2.common.interfaces";
        internal const string kInterfaceName = "ILoadUriListener";

        private Action<byte[]> m_callback;

        public LoadURIProxyListener(Action<byte[]> callback) : base(kPackage + "." + kInterfaceName)
        {
            m_callback = callback;
        }

        private void onLoadComplete(byte[] data)
        {
            if (m_callback != null)
            {
                m_callback(data);
            }
        }

        public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
        {
            if (methodName == "onLoadComplete")
            {
                onLoadComplete(AndroidJNIHelper.ConvertFromJNIArray<byte[]>(javaArgs[0].GetRawObject()));
                return null;
            }
            else
                return base.Invoke(methodName, javaArgs);
        }
    }
}
#endif
