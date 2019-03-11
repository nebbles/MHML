using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    internal abstract class NativeShareSheetInterfaceBase : NativeInterfaceObject, INativeShareSheetInterface
    {
        #region INativeShareSheetInterface implementation

        protected ShareSheetClosedCallback m_onCloseEvent;
        public event ShareSheetClosedCallback onClose
        {
            add
            {
                m_onCloseEvent    += value;
            }
            remove
            {
                m_onCloseEvent    -= value;
            }
        }

        public abstract void AddText(string text);
        
        public abstract void AddScreenshot();
        
        public abstract void AddImage(Texture2D image);
        
        public abstract void AddURL(URLString url);

        public abstract void Show(Vector2 screenPosition);

        #endregion
    }
}