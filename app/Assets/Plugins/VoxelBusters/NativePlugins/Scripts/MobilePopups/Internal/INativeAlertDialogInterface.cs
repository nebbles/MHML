using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    // delegate signatures
    internal delegate void ButtonClickCallback(int selectedButtonIndex);

    internal interface INativeAlertDialogInterface
    {   
        #region Events

        event ButtonClickCallback onButtonClick;

        #endregion

        #region Methods

        // setters and getter methods
        void SetTitle(string value);

        string GetTitle();

        void SetMessage(string value);

        string GetMessage();

        // action methods
        void AddButton(string text, bool isCancelType);

        // presentation methods
        void Show();
        
        void Dismiss();

        #endregion
    }
}