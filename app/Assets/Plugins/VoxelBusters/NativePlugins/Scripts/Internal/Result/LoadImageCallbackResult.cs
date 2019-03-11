using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal class LoadImageCallbackResult : CallbackResultBase, ILoadImageCallbackResult
    {
        #region ILoadImageCallbackResult implementation

        public Texture2D Image
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public LoadImageCallbackResult(Texture2D image, string error)
        {
            // set properties
            Image   = image;
            Error   = error;
        }

        #endregion
    }
}