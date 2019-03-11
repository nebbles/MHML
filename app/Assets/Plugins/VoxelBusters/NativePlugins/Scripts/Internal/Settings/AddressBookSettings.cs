using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    [Serializable]
    public class AddressBookSettings : NativeFeatureSettingsBase
    {
        #region Fields

        [SerializeField]
        private     Texture2D               m_defaultImage      = null;
        [SerializeField]
        private     UsagePermission         m_usagePermission   = UsagePermission.Create(RuntimePlatformValue.All("$productName uses contacts."));

        #endregion

        #region Properties

        public Texture2D DefaultImage
        {
            get
            {
                return m_defaultImage;
            }
        }

        public UsagePermission UsagePermission
        {
            get
            {
                return m_usagePermission;
            }
        }

        #endregion
    }
}