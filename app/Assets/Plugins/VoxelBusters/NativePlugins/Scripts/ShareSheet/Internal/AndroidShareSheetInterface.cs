#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    using Internal;
    internal partial class AndroidShareSheetInterface : NativeShareSheetInterfaceBase
    {
        #region Properties

        private AndroidJavaObject Plugin
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public AndroidShareSheetInterface()
        {
            Plugin = AndroidPluginUtility.CreateJavaInstance(Native.kClassName);
        }

        #endregion

        #region INativeShareSheetInterface Implementation

        public override void AddImage(Texture2D image)
        {
            byte[] data = ImageConversion.EncodeToPNG(image);
            Plugin.Call(Native.Method.kAddAttachmentData, data, MimeType.kPNGImage, "share.png");
        }

        public override void AddScreenshot()
        {
            string filePath = UnityEngineUtility.TakeScreenshot("screenshot.png");
            Plugin.Call(Native.Method.kAddFileAtPathAsync, filePath, MimeType.kPNGImage);
        }

        public override void AddText(string text)
        {
            Plugin.Call(Native.Method.kSetMessage, text, false);
        }

        public override void AddURL(URLString url)
        {
            Plugin.Call(Native.Method.kSetURL, url.ToString());
        }

        public override void Show(Vector2 screenPosition)
        {
            Plugin.Call(Native.Method.kShow, new ShareSheetProxyListener(m_onCloseEvent));
        }

        protected override void InvalidateInternal()
        {
            base.InvalidateInternal();
        }

        #endregion
    }
}
#endif