#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using AOT;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    internal class IOSShareSheetInterface : NativeShareSheetInterfaceBase, INativeShareSheetInterface
    {
        #region Fields

        private     IntPtr          m_nativePtr;

        #endregion

        #region Delegates

        internal delegate void ShareSheetClosedNativeCallback(IntPtr nativePtr, bool completed, string error);

        #endregion

        #region Binding calls

        [DllImport("__Internal")]
        private static extern void NPShareSheetRegisterCallback(ShareSheetClosedNativeCallback closedCallback);

        [DllImport("__Internal")]
        private static extern IntPtr NPShareSheetCreate();

        [DllImport("__Internal")]
        private static extern void NPShareSheetShow(IntPtr nativePtr, float posX, float posY);
        
        [DllImport("__Internal")]
        private static extern void NPShareSheetDestroy(IntPtr nativePtr);

        [DllImport("__Internal")]
        private static extern void NPShareSheetAddText(IntPtr nativePtr, string value);
        
        [DllImport("__Internal")]
        private static extern void NPShareSheetAddScreenshot(IntPtr nativePtr);
        
        [DllImport("__Internal")]
        private static extern void NPShareSheetAddImage(IntPtr nativePtr, IntPtr dataArrayPtr, int dataLength);
        
        [DllImport("__Internal")]
        private static extern void NPShareSheetAddURL(IntPtr nativePtr, string url);

        #endregion

        #region Constructors

        static IOSShareSheetInterface()
        {
            // register callbacks
            NPShareSheetRegisterCallback(closedCallback: HandleShareSheetClosedCallbackInternal);
        }

        public IOSShareSheetInterface()
        {
             // create object
            IntPtr composerPtr = NPShareSheetCreate();
            if (IntPtr.Zero == composerPtr)
            {
                throw ErrorCentre.CreateNativeObjectFailedException();
            }

            // set properties
            m_nativePtr     = composerPtr;

            // track instance
            NativePluginsInstanceMap.AddInstance(m_nativePtr, this);
        }

        #endregion

        #region Base class implementation

        public override void AddText(string text)
        {
            NPShareSheetAddText(m_nativePtr, text);
        }

        public override void AddScreenshot()
        {
            NPShareSheetAddScreenshot(m_nativePtr);
        }

        public override void AddImage(Texture2D image)
        {
            // get data
            string      mimeType;
            byte[]      data        = image.EncodeTexture(out mimeType);
            GCHandle    handle      = GCHandle.Alloc(data, GCHandleType.Pinned);

            // send data
            NPShareSheetAddImage(m_nativePtr, handle.AddrOfPinnedObject(), data.Length);

            // release pointer
            handle.Free();
        }

        public override void AddURL(URLString url)
        {
            NPShareSheetAddURL(m_nativePtr, url.ToString());
        }

        public override void Show(Vector2 screenPosition)
        {
            Vector2     invertedPosition    = UnityEngineUtility.InvertScreenPosition(screenPosition, invertY: true);
            NPShareSheetShow(m_nativePtr, invertedPosition.x, invertedPosition.y);
        }

        protected override void InvalidateInternal()
        {
            base.InvalidateInternal();
            
            // reset
            NativePluginsInstanceMap.RemoveInstance(m_nativePtr);
            NPShareSheetDestroy(m_nativePtr);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(ShareSheetClosedNativeCallback))]
        private static void HandleShareSheetClosedCallbackInternal(IntPtr nativePtr, bool completed, string error)
        {
            IOSShareSheetInterface owner = NativePluginsInstanceMap.GetOwner<IOSShareSheetInterface>(nativePtr);
            if (null == owner)
            {
                throw ErrorCentre.NullReferenceException("owner");
            }
            if (error != null)
            {
                Debug.Log("[NativePlugins] The requested operation failed with error. Error description: " + error);
            }

            // send callback
            ShareSheetClosedCallbackResult callbackResult = new ShareSheetClosedCallbackResult()
            {
                ResultCode = completed ? ShareSheetResultCode.Done : ShareSheetResultCode.Cancelled,
            };
            owner.m_onCloseEvent(callbackResult);
        }

        #endregion
    }
}
#endif