using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.iOS
{
    public class ExternalServiceProvider
    {
        #region Static fields

        public static IJsonServiceProvider JsonServiceProvider
        {
            get;
            set;
        }

        #endregion
    }
}