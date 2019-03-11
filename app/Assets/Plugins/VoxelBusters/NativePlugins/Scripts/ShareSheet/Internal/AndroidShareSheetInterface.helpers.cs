#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    using Internal;
    internal partial class AndroidShareSheetInterface : NativeShareSheetInterfaceBase
    {
        #region Native platform Info

        private class Native
        {
            internal const string kPackage                      = "com.voxelbusters.nativeplugins.v2.features.sharing";
            internal const string kClassName                    = kPackage + "." + "ShareSheet";
            internal const string kShareSheetListenerInterface  = kPackage + "." + "ISharing$IShareSheetListener";

            internal class Method
            {
                internal const string kSetMessage               = "setMessage";
                internal const string kAddAttachmentData        = "addAttachmentData";
                internal const string kAddFileAtPathAsync       = "addFileAtPathAsync";
                internal const string kSetURL                   = "setUrl";
                internal const string kShow                     = "show";
            }
        }

        #endregion

        #region Proxy listeners

        internal class ShareSheetProxyListener : NativeProxy<ShareSheetClosedCallback>
        {

            #region Constructors

            public ShareSheetProxyListener(ShareSheetClosedCallback callback) : base(callback, Native.kShareSheetListenerInterface)
            {
            }

            #endregion

            #region Callbacks

            private void onAction(AndroidShareSheetResultCode resultCode)
            {
                ShareSheetClosedCallbackResult callbackResult = new ShareSheetClosedCallbackResult()
                {
                    ResultCode = Convert(resultCode),
                    Error = null
                };

                if (m_callback != null)
                {
                    DispatchOnMainThread(() => m_callback(callbackResult));
                }
            }

            public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
            {
                if (methodName == "onAction")
                {
                    AndroidShareSheetResultCode resultCode = (AndroidShareSheetResultCode)javaArgs[0].Call<int>("ordinal");

                    onAction(resultCode);
                    return null;
                }
                else
                {
                    return base.Invoke(methodName, javaArgs);
                }
            }

            #endregion

            #region Helpers

            private ShareSheetResultCode Convert(AndroidShareSheetResultCode resultCode)
            {
                switch (resultCode)
                {
                    case AndroidShareSheetResultCode.Cancelled:
                        return ShareSheetResultCode.Cancelled;

                    case AndroidShareSheetResultCode.Done:
                        return ShareSheetResultCode.Done;

                    case AndroidShareSheetResultCode.Unknown:
                        return ShareSheetResultCode.Unknown;

                    default:
                        throw ErrorCentre.SwitchCaseNotImplementedException(resultCode);
                }
            }

            #endregion
        }

        #endregion

        #region Data types

        internal enum AndroidShareSheetResultCode
        {
            Cancelled,
            Done,
            Unknown
        }

        #endregion
    }
}
#endif