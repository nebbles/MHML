using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
    [Serializable]
    public class SharingSettings : NativeFeatureSettingsBase
    {
        #region Properties

        [SerializeField]
        private         bool        m_usesMailComposer              = true;
        [SerializeField]
        private         bool        m_usesMessageComposer           = true;
        [SerializeField]
        private         bool        m_usesSocialShareComposer       = true;
        [SerializeField]
        private         bool        m_usesShareSheet                = true;

        #endregion

        #region Properties

        public bool UsesMailComposer
        {
            get
            {
                return m_usesMailComposer;
            }
        }

        public bool UsesMessageComposer
        {
            get
            {
                return m_usesMessageComposer;
            }
        }

        public bool UsesSocialShareComposer
        {
            get
            {
                return m_usesSocialShareComposer;
            }
        }

        public bool UsesShareSheet
        {
            get
            {
                return m_usesShareSheet;
            }
        }
        
        #endregion
    }
}