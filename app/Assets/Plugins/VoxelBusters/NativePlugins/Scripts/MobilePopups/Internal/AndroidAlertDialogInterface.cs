#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    using Internal;
    internal partial class AndroidAlertDialogInterface : NativeAlertDialogInterfaceBase, INativeAlertDialogInterface
    {
        #region Properties

        private AndroidJavaObject Plugin
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public AndroidAlertDialogInterface()
        {
            Plugin = AndroidPluginUtility.CreateJavaInstance(Native.kClassName);
        }

        #endregion

        #region INativeAlertDialogInterface implementation

        public override void AddButton(string text, bool isCancelType)
        {
            Plugin.Call(Native.Method.kAddButton, text, isCancelType);
        }

        public override string GetMessage()
        {
            return Plugin.Call<string>(Native.Method.kGetMessage);
        }

        public override string GetTitle()
        {
            return Plugin.Call<string>(Native.Method.kGetTitle);
        }

        public override void SetMessage(string value)
        {
            Plugin.Call(Native.Method.kSetMessage, value);
        }

        public override void SetTitle(string value)
        {
            Plugin.Call(Native.Method.kSetTitle, value);
        }

        public override void Show()
        {
            Plugin.Call(Native.Method.kShow, new ButtonClickProxyListener(onButtonClickEvent));
        }

        public override void Dismiss()
        {
            Plugin.Call(Native.Method.kDismiss); 
        }

        protected override void InvalidateInternal()
        {
            base.InvalidateInternal();
        }

        #endregion
    }
}
#endif