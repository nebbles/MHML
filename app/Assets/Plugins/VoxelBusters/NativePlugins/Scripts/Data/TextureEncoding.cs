using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// This enum is used to define the texture encoding technique to be used by the plugin.
    /// </summary>
    public enum TextureEncoding
    {
        /// <summary> Encodes the given texture into PNG format.</summary>
        ToPNG,

        /// <summary> Encodes the given texture into JPEG format.</summary>
        ToJPG,
    }
}