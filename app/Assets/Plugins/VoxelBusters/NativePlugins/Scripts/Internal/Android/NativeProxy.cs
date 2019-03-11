#if UNITY_ANDROID
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    public class NativeProxy<T> : AndroidJavaProxy
    {
        protected T m_callback;

        public NativeProxy(T m_callback, string interfaceName) : base(interfaceName)
        {
            this.m_callback = m_callback;
        }

        protected void DispatchOnMainThread(Action action)
        {
            // Dispatch on Unity Thread
            UnityThreadDispatcher.Instance.Enqueue(action);
        }
    }
}
#endif
