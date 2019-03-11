using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Android
{
    public class Provider : Element
    {
        public void Add(MetaData element)
        {
            base.Add(element);
        }

        protected override string GetName()
        {
            return "provider";
        }
    }
}
