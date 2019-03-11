using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Android
{
    public class Activity : AppComponent
    {
        public Layout Layout
        {
            get;
            set;
        }

        protected override string GetName()
        {
            return "activity";
        }
    }
}

