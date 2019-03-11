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
    internal class IOSAlertDialogInterface : NativeAlertDialogInterfaceBase, INativeAlertDialogInterface
    {
        #region Fields

        private     IntPtr      m_nativePtr;

        #endregion

        #region Delegates

        public delegate void NativeButtonClickCallback(IntPtr nativePtr, int selectedButtonIndex);

        #endregion

        #region Binding calls

        [DllImport("__Internal")]
        private static extern void NPAlertDialogRegisterCallback(NativeButtonClickCallback clickCallback);

        [DllImport("__Internal")]
        private static extern IntPtr NPAlertDialogCreate(string title, string message, UIAlertControllerStyle preferredStyle);

        [DllImport("__Internal")]
        private static extern void NPAlertDialogShow(IntPtr nativePtr);

        [DllImport("__Internal")]
        private static extern void NPAlertDialogDismiss(IntPtr nativePtr);

        [DllImport("__Internal")]
        private static extern void NPAlertDialogDestroy(IntPtr nativePtr);

        [DllImport("__Internal")]
        private static extern void NPAlertDialogSetTitle(IntPtr nativePtr, string value);

        [DllImport("__Internal")]
        private static extern string NPAlertDialogGetTitle(IntPtr nativePtr);

        [DllImport("__Internal")]
        private static extern void NPAlertDialogSetMessage(IntPtr nativePtr, string value);

        [DllImport("__Internal")]
        private static extern string NPAlertDialogGetMessage(IntPtr nativePtr);
        
        [DllImport("__Internal")]
        private static extern void NPAlertDialogAddAction(IntPtr nativePtr, string text, bool isCancelType);

        #endregion

        #region Constructors

        static IOSAlertDialogInterface()
        {
            // initialise component
            NPAlertDialogRegisterCallback(clickCallback: HandleButtonClickCallbackInternal);
        }

        internal IOSAlertDialogInterface()
        {
            // prepare component
            IntPtr  nativePtr   = NPAlertDialogCreate(title: string.Empty, message: string.Empty, preferredStyle: UIAlertControllerStyle.UIAlertControllerStyleAlert);
            if (IntPtr.Zero == nativePtr)
            {
                throw new Exception("Failed to create alert dialog");
            }
            // set property
            m_nativePtr     = nativePtr;

            // add to collection to map action
            NativePluginsInstanceMap.AddInstance(m_nativePtr, this);
        }

        #endregion

        #region Base class implementation

        public override void SetTitle(string value)
        {
            NPAlertDialogSetTitle(m_nativePtr, value);
        }

        public override string GetTitle()
        {
            return NPAlertDialogGetTitle(m_nativePtr);
        }
            
        public override void SetMessage(string value)
        {
            NPAlertDialogSetMessage(m_nativePtr, value);
        }

        public override string GetMessage()
        {
            return NPAlertDialogGetMessage(m_nativePtr);
        }

        public override void AddButton(string text, bool isCancelType)
        {
            NPAlertDialogAddAction(m_nativePtr, text, isCancelType);
        }

        public override void Show()
        {
            NPAlertDialogShow(m_nativePtr);
        }

        public override void Dismiss()
        {
            NPAlertDialogDismiss(m_nativePtr);
        }

        protected override void InvalidateInternal()
        {
            base.InvalidateInternal();
            
            // reset internal properties
            NativePluginsInstanceMap.RemoveInstance(m_nativePtr);
            NPAlertDialogDestroy(m_nativePtr);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(NativeButtonClickCallback))]
        private static void HandleButtonClickCallbackInternal(IntPtr nativePtr, int selectedButtonIndex)
        {
            IOSAlertDialogInterface owner = NativePluginsInstanceMap.GetOwner<IOSAlertDialogInterface>(nativePtr);
            if (null == owner)
            {
                throw ErrorCentre.NullReferenceException("owner");
            }
            owner.onButtonClickEvent(selectedButtonIndex);
        }

        #endregion

        #region Nested types

        private enum UIAlertControllerStyle
        {
            UIAlertControllerStyleActionSheet = 0,
            UIAlertControllerStyleAlert
        }

        #endregion
    }
}
#endif