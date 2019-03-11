using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Android
{
    public class Library : Element
    {
        protected override string GetName()
        {
            return "uses-library";
        }
    }
}

