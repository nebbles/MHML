using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal class CallbackResultBase : ICallbackResult
    {
        #region ICallbackResult implementation

        public string Error
        {
            get;
            internal set;
        }

        #endregion
    }
}