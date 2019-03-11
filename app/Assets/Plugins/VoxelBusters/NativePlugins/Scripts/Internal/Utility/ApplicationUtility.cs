using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ApplicationRuntimePlatform = UnityEngine.RuntimePlatform;

namespace VoxelBusters.NativePlugins.Internal
{
    internal static class ApplicationUtility
    {
        #region Static methods

        public static string GetBundleVersion()
        {
            return Application.version;
        }

        public static string GetBundleIdentifier()
        {
            return Application.identifier;
        }

        internal static RuntimePlatform GetRuntimePlatform()
        {
            switch (Application.platform)
            {
                case ApplicationRuntimePlatform.IPhonePlayer:
                    return RuntimePlatform.iOS;

                case ApplicationRuntimePlatform.tvOS:
                    return RuntimePlatform.tvOS;

                case ApplicationRuntimePlatform.Android:
                    return RuntimePlatform.Android;

                default:
                    return RuntimePlatform.Unknown;
            }
        }

        #endregion
    }
}