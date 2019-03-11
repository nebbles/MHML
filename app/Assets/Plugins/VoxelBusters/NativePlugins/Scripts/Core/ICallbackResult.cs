using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    /// <summary>
    /// Base interface to be implemented by the result object returned by the native operation.
    /// </summary>
    public interface ICallbackResult
    {
        #region Properties

        /// <summary>
        /// Provides additional information about the kind of error and any underlying cause occured while executing native operation.
        /// </summary>
        /// <value>If the requested operation was successful, this value is null; otherwise, this property holds the description of the problem that occurred.</value>
        string Error
        {
            get;
        }

        #endregion
    }
}