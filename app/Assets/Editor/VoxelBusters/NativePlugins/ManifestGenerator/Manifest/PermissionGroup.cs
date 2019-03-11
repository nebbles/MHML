using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Android
{
    public class PermissionGroup : Element
    {
        protected override string GetName()
        {
            return "permission-group";
        }
    }
}