using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Android
{
    public class SupportedGLTexture : Element
    {
        protected override string GetName()
        {
            return "supports-gl-texture";
        }
    }
}