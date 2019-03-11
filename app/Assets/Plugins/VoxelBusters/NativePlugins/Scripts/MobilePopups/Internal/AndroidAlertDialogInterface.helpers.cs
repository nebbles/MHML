#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    using Internal;

    internal partial class AndroidAlertDialogInterface : NativeAlertDialogInterfaceBase, INativeAlertDialogInterface
    {
        #region Platform native Info

        private class Native
        {
            #region Constants

            internal const string kPackage                  = "com.voxelbusters.nativeplugins.v2.features.popups";
            internal const string kClassName                = kPackage + "." + "AlertView";
            internal const string kButtonListenerInterface  = kPackage + "." + "IPopups$IButtonClickListener";

            #endregion

            #region Nested types

            internal class Method
            {
                internal const string kAddButton            = "addButton";
                internal const string kGetMessage           = "getMessage";
                internal const string kGetTitle             = "getTitle";
                internal const string kSetMessage           = "setMessage";
                internal const string kSetTitle             = "setTitle";
                internal const string kDismiss              = "dismiss";
                internal const string kShow                 = "show";
            }

            #endregion
        }

        #endregion

        #region Proxy listeners

        internal class ButtonClickProxyListener : NativeProxy<ButtonClickCallback>
        {
            #region Constructors

            public ButtonClickProxyListener(ButtonClickCallback callback) : base(callback, Native.kButtonListenerInterface)
            {
            }

            #endregion

            #region Callbacks

            private void onClick(int index)
            {
                if (m_callback != null)
                {
                    DispatchOnMainThread(() => m_callback(index));
                }
            }

            #endregion
        }

        #endregion
    }
}
#endif