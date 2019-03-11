using System;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// The enum is used to indicate the platform application is running.
    /// </summary>
    [Flags]
    public enum RuntimePlatform
    {
        /// <summary> The runtime platform could not be determined.</summary>
        Unknown     = 0,

        /// <summary> The runtime platform is iOS.</summary>
        iOS         = 1 << 1,

        /// <summary> The runtime platform is tvOS.</summary>
        tvOS        = 1 << 2,

        /// <summary> The runtime platform is Android.</summary>
        Android     = 1 << 3,

        /// <summary> The runtime platform is Unity Editor.</summary>
        Editor      = 1 << 4,

        All         =  iOS | tvOS | Android | Editor,
    }
}