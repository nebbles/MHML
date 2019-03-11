using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    public class GenericCallbackUtility : MonoBehaviour
    {
        #region Methods

        public static void InvokeCallbackInSafeMode<T>(object callback, T result) where T : ICallbackResult
        {
            try
            {
                ((GenericCallback<T>)callback).Invoke(result);
            }
            catch (Exception)
            {}
        }

        #endregion
    }
}