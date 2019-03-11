#if UNITY_IOS
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOT;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    internal static class IOSNativeCommonOperations
    {
        #region Native binding methods
        
        [DllImport("__Internal")]
        private static extern void NPUtilityRegisterCallbacks(LoadTextureNativeCallback loadTextureCallback);
        
        [DllImport("__Internal")]
        private static extern void NPUtilityLoadTexture(IntPtr nativeDataPtr, IntPtr tagPtr);

        #endregion

        #region Delegates

        public delegate void LoadTextureNativeCallback(IntPtr byteArrayPtr, int byteLength, IntPtr tagPtr);

        #endregion

        #region Constructors

        static IOSNativeCommonOperations()
        {
            // initialise
            NPUtilityRegisterCallbacks(loadTextureCallback: HandleLoadTextueCallbackInternal);
        }

        #endregion

        #region Load methods

        public static void LoadImage(IntPtr nativePtr, GenericCallback<ILoadImageCallbackResult> callback)
        {
            // check arguments
            if (null == callback)
            {
                throw ErrorCentre.ArgumentNullException("callback");
            }

            // save callback as handle
            IntPtr callbackPtr = MarshalUtility.GetIntPtr(callback);
            NPUtilityLoadTexture(nativePtr, callbackPtr);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(LoadTextureNativeCallback))]
        private static void HandleLoadTextueCallbackInternal(IntPtr dataArrayPtr, int dataLength, IntPtr tagPtr)
        {
            // get handle from pointer
            GCHandle                                    handle      = GCHandle.FromIntPtr(tagPtr);
            GenericCallback<ILoadImageCallbackResult>   callback    = handle.Target as GenericCallback<ILoadImageCallbackResult>;

            try
            {
                if (IntPtr.Zero == dataArrayPtr)
                {
                    // send callback with error info
                    LoadImageCallbackResult callbackResult = new LoadImageCallbackResult(null, "Could not load texture data.");
                    if (callback != null)
                    {
                        callback(callbackResult);
                    }
                }
                else
                {
                    // create texture from raw data
                    byte[]      byteArray   = new byte[dataLength];
                    Marshal.Copy(dataArrayPtr, byteArray, 0, dataLength);

                    Texture2D   newTexture  = new Texture2D(4, 4);
                    newTexture.LoadImage(byteArray);
                    newTexture.Apply();

                    // send callback with image data
                    LoadImageCallbackResult callbackResult = new LoadImageCallbackResult(newTexture, null);
                    if (callback != null)
                    {
                        callback(callbackResult);
                    }
                }
            }
            finally
            {
                // release handle
                handle.Free();
            }
        }

        #endregion
    }
}
#endif