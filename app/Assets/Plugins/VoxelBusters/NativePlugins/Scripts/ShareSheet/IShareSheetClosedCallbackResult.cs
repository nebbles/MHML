using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// This interface contains the result of the user action which caused <see cref="ShareSheet"/> interface to dismiss.
    /// </summary>
    public interface IShareSheetClosedCallbackResult : ICallbackResult
    {
        #region Properties

        /// <summary>
        /// Gets the result of the user’s action.
        /// </summary>
        /// <value>The result code of user’s action.</value>
        ShareSheetResultCode ResultCode
        {
            get;
        }

        #endregion
    }
}