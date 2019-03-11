using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{

    /// <summary>
    /// A zero argument generic callback.
    /// </summary>
    public delegate void GenericCallback();


    /// <summary>
    /// Generic callback contains the result returned by the native operation.
    /// </summary>
    public delegate void GenericCallback<T>(T result) where T : ICallbackResult;
}