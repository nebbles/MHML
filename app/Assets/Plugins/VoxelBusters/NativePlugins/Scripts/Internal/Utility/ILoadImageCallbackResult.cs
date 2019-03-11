using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    public interface ILoadImageCallbackResult : ICallbackResult
    {
        #region Properties

        Texture2D Image
        {
            get;
        }

        #endregion
    }
}