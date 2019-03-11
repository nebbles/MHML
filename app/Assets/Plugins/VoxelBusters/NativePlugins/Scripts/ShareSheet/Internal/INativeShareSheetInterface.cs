using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    // delegate signature
    internal delegate void ShareSheetClosedCallback(IShareSheetClosedCallbackResult result);

    internal interface INativeShareSheetInterface
    {
        #region Events

        event ShareSheetClosedCallback onClose;

        #endregion

        #region Methods

        // attachment methods
        void AddText(string text);

        void AddScreenshot();
        
        void AddImage(Texture2D image);
        
        void AddURL(URLString url);

        // presentation methods
        void Show(Vector2 screenPosition);

        #endregion
    }
}