using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins.Internal
{
    public static class NativePluginsUtility
    {
        #region Static Methods

        public static RuntimePlatform GetActivePlatform()
        {
#if UNITY_EDITOR
            return UnityEditorUtility.GetRuntimePlatform();
#else
            return ApplicationUtility.GetRuntimePlatform();
#endif
        }

        #endregion

        #region Runtime platform methods

        public static string FindValueForActivePlatform(RuntimePlatformValue[] array)
        {
            RuntimePlatform activePlatform = GetActivePlatform();
            return FindValueForPlatform(array, activePlatform);
        }

        public static string FindValueForPlatform(RuntimePlatformValue[] array, RuntimePlatform platform)
        {
            if (array != null)
            {
                RuntimePlatformValue value = Array.Find(array, (item) => item.IsEqualToPlatform(platform));
                if (value != null)
                {
                    return value.ToString();
                }
            }
            return null;
        }

        #endregion
    }
}