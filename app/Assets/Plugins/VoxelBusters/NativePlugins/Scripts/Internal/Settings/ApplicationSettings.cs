using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    [Serializable]
    public class ApplicationSettings
    {
        #region Fields

        [SerializeField, Tooltip("The string that identifies your app in Google Play Store.")]
        private     RuntimePlatformValue[]      m_appStoreIds;

        #endregion

        #region Public methods

        public string GetAppStoreIdForPlatform(RuntimePlatform platform)
        {
            return NativePluginsUtility.FindValueForPlatform(m_appStoreIds, platform);
        }

        public string GetAppStoreIdForActivePlatform()
        {
            return NativePluginsUtility.FindValueForActivePlatform(m_appStoreIds);
        }

        #endregion
    }
}