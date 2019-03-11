#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal class EditorShareSheetInterface : NativeShareSheetInterfaceBase, INativeShareSheetInterface
    {
       #region Base class implementation

        public override void AddText(string text)
        {}

        public override void AddScreenshot()
        {}

        public override void AddImage(Texture2D image)
        {}

        public override void AddURL(URLString url)
        {}

        public override void Show(Vector2 screenPosition)
        {
            ErrorCentre.LogNotSupportedInEditor(featureName: "ShareSheet");

            // send callback
            ShareSheetClosedCallbackResult callbackResult = new ShareSheetClosedCallbackResult()
            {
                ResultCode = ShareSheetResultCode.Cancelled,
            };
            m_onCloseEvent(callbackResult);
        }

        #endregion
    }
}
#endif