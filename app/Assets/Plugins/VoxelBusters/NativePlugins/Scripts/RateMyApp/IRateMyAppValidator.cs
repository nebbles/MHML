using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    public interface IRateMyAppValidator
    {
        #region Methods

        // check status
        bool CanShowRateMyApp();

        // external events
        void DidClickOnRemindLaterButton();

        void DidClickOnCancelButton();

        void DidClickOnOkButton();
        
        #endregion
    }
}