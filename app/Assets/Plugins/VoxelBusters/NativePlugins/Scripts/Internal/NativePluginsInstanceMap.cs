using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    internal static class NativePluginsInstanceMap
    {
        #region Static fields

        private static Dictionary<IntPtr, object> instanceMap;

        #endregion

        #region Constructors

        static NativePluginsInstanceMap()
        {
            // set properties
            instanceMap = new Dictionary<IntPtr, object>(capacity: 4);
        }

        #endregion

        #region Internal methods

        internal static void AddInstance(IntPtr nativePtr, object owner)
        {
            instanceMap.Add(nativePtr, owner);
        }

        internal static void RemoveInstance(IntPtr nativePtr)
        {
            instanceMap.Remove(nativePtr);
        }

        internal static T GetOwner<T>(IntPtr nativePtr) where T : class
        {
            object owner;
            instanceMap.TryGetValue(nativePtr, out owner);

            return owner as T;
        }

        #endregion
    }
}