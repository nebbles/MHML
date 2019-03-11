using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal class ShareSheetClosedCallbackResult : CallbackResultBase, IShareSheetClosedCallbackResult
    {
        #region IShareSheetClosedCallbackResult implementation

        public ShareSheetResultCode ResultCode
        {
            get;
            internal set;
        }

        #endregion
    }
}